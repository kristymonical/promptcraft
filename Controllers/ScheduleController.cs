using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aethon;
using Microsoft.AspNetCore.Mvc;
using SVT.Platform.Commands;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Controllers
{
    [Route("api")]
    public class ScheduleController : ControllerBase
    {
        private SVTContext _svtContext;
        private AethonApi _aethonApi;
        // @TODO: remove once configuration is implemented
        private Dictionary<int, int> _poolThresholds = new Dictionary<int, int>{
            { 1, 2 },
            { 2, 1 },
            { 3, 1 }
        };

        private TimeSpan _timeout;

        public ScheduleController(SVTContext svtContext, AethonApi aethonApi, int timeout = 15)
        {
            _svtContext = svtContext;
            _aethonApi = aethonApi;
            _timeout = TimeSpan.FromSeconds(timeout);
        }

        [HttpPost("delivery/schedule")]
        public async Task<IActionResult> ScheduleJob()
        {
            var trackingId = Guid.NewGuid().ToString();
            // @TODO: do we need to load 'User'/'ActionType' entities?
            var user = "ScheduleService";
            var action = "schedule";

            await LogCommands.CreateLog<BaseLogData>(_svtContext, new DataToLog<BaseLogData>
            {
                TrackingId = trackingId,
                Action = action,
                Data = new BaseLogData
                {
                    User = user,
                    Message = $"Running Delivery Scheduling Service: {DateTime.UtcNow}"
                }
            });
            await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

            // @TODO: implement configurable pool threshold values
            foreach (var pool in _poolThresholds.Keys)
            {
                var threshold = _poolThresholds.GetValueOrDefault(pool);
                var deliveryQueueEmpty = false;
                var activeJobCount = await JobCommands.GetActiveJobCountByPool(_svtContext, pool);
                var queueOffset = 0;

                var currentCount = activeJobCount;
                while (!deliveryQueueEmpty && currentCount < threshold)
                {
                    var currentDelivery = (await DeliveryCommands.GetDeliveryQueueInPriorityOrder(_svtContext, pool))
                        .ElementAtOrDefault(queueOffset);

                    if (currentDelivery == null)
                    {
                        deliveryQueueEmpty = true;
                        continue;
                    }

                    var startingLocation = currentDelivery.Locations.FirstOrDefault();
                    var destinationArea = AreaCommands.GetIntermediateArea(startingLocation.Area, currentDelivery.DestinationArea);

                    if (startingLocation == null || destinationArea == null)
                    {
                        await LogCommands.CreateLog<InvalidDestinationLog>(_svtContext, new DataToLog<InvalidDestinationLog>
                        {
                            TrackingId = trackingId,
                            Action = action,
                            Delivery = currentDelivery,
                            Data = new InvalidDestinationLog
                            {
                                User = user,
                                Message = $"Invalid Data, Delivery: {currentDelivery.DeliveryId}",
                                StartingLocation = startingLocation.Name,
                                DestinationArea = destinationArea.Name
                            }
                        });
                        await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                        // @TODO: implement possible bad data alerting here

                        currentDelivery.Canceled = DateTime.UtcNow;
                        await DeliveryCommands.PopDeliveryQueue(_svtContext, pool);
                        await _svtContext.SaveChangesAsync();
                        continue;
                    }

                    var destinationLocation = LocationCommands.GetDeliverableLocationByArea(destinationArea);

                    if (destinationLocation == null)
                    {
                        await LogCommands.CreateLog<NoAvailableLocationsLog>(_svtContext, new DataToLog<NoAvailableLocationsLog>
                        {
                            TrackingId = trackingId,
                            Action = action,
                            Delivery = currentDelivery,
                            Data = new NoAvailableLocationsLog
                            {
                                User = user,
                                Message = $"No Available Locations, Delivery: {currentDelivery.DeliveryId}",
                                DestinationArea = destinationArea.Name
                            }
                        });
                        await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                        // @TODO: implement no available destination locations alerting here

                        queueOffset += 1;
                        continue;
                    }

                    destinationLocation.Reserved = true;
                    currentDelivery.Locations.Add(destinationLocation);
                    await _svtContext.SaveChangesAsync();

                    (bool success, int aethonJobId) = await JobCommands.ScheduleTug(_aethonApi, new MultiDestinationRequest
                    {
                        PoolId = pool,
                        GroupId = 1,
                        Timeout = -1,
                        Destinations = new string[] { startingLocation.Name, destinationLocation.Name }
                    });

                    if (!success || aethonJobId == -1)
                    {
                        destinationLocation.Reserved = false;
                        currentDelivery.Locations.Remove(destinationLocation);
                        await _svtContext.SaveChangesAsync();

                        // @TODO: log unexpected Aethon error to table here (must be AFTER above .SaveChanges())

                        queueOffset += 1;
                        continue;
                    }

                    await LogCommands.CreateLog(_svtContext, new DataToLog<AethonSendLog>
                    {
                        TrackingId = trackingId,
                        Action = action,
                        Delivery = currentDelivery,
                        Data = new AethonSendLog
                        {
                            User = user,
                            Message = $"Aethon Send - From: '{startingLocation.Name}', To: '{destinationLocation.Name}'",
                            Success = success,
                            AethonJobId = aethonJobId
                        }
                    });
                    await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                    currentDelivery.Jobs.Add(new Job { AethonJobId = aethonJobId });

                    await DeliveryCommands.PopDeliveryQueue(_svtContext, pool);
                    await _svtContext.SaveChangesAsync();

                    currentCount = await JobCommands.GetActiveJobCountByPool(_svtContext, pool);
                }
            }

            return Ok(new { success = true, message = "" });
        }
    }
}