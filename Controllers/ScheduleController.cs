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
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using static SVT.Platform.Data.Models.Area;

namespace SVT.Platform.Controllers
{
    [Route("api")]
    public class ScheduleController : ControllerBase
    {
        private SVTContext _svtContext;
        private AethonApi _aethonApi;
        private IWebHostEnvironment _environment;
        private User _user;
        private ActionType _action;
        // @TODO: remove once configuration is implemented
        private Dictionary<int, int> _poolThresholds = new Dictionary<int, int>{
            { 1, 2 },
            { 2, 1 },
            { 3, 1 }
        };

        private TimeSpan _timeout;
        // @TODO: add constant value to config
        private string _scheduleHealthCheckTrackingId = "7ac0fc0d-7176-4b71-ae09-b80f2515a9da";

        public ScheduleController(SVTContext svtContext, AethonApi aethonApi, IWebHostEnvironment environment, int timeout = 15)
        {
            _svtContext = svtContext;
            _aethonApi = aethonApi;
            _timeout = TimeSpan.FromSeconds(timeout);
            _environment = environment;
            _user = UserCommands.GetUserByName(_svtContext, "ScheduleService");
            _action = ActionCommands.GetActionByValue(_svtContext, "schedule");
        }

        [HttpPost("delivery/schedule")]
        public async Task<IActionResult> ScheduleJob()
        {
            var healthCheckLog = await _svtContext.Logs
                .Where(l => l.TrackingId == _scheduleHealthCheckTrackingId)
                .FirstOrDefaultAsync();

            var logDataToSerialize = new BaseLogData
            {
                User = _user.Name,
                Message = $"Running Delivery Scheduling Service: {DateTime.UtcNow}"
            };

            if (healthCheckLog == null)
            {
                await LogCommands.CreateLog<BaseLogData>(_svtContext, new DataToLog<BaseLogData>
                {
                    TrackingId = _scheduleHealthCheckTrackingId,
                    Action = _action.Value,
                    Data = logDataToSerialize
                });
            }
            else
            {
                healthCheckLog.Serialized = JsonConvert.SerializeObject(logDataToSerialize);
            }
            await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

            var trackingId = Guid.NewGuid().ToString();

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

                    GraphDirection direction;

                    if (currentDelivery.DeliveryType == "return") direction = GraphDirection.descendants;
                    else direction = GraphDirection.ancestors;

                    var destinationArea = currentDelivery.DestinationArea
                        .GetAreasBy(direction, (final, current) =>
                            startingLocation.Area != current &&
                            current.IsAdjacentTo(direction, startingLocation.Area) &&
                            current.Pool == startingLocation.Area.Pool)
                        .FirstOrDefault();

                    if (startingLocation == null || destinationArea == null)
                    {
                        await LogCommands.CreateLog<InvalidDestinationLog>(_svtContext, new DataToLog<InvalidDestinationLog>
                        {
                            TrackingId = trackingId,
                            Action = _action.Value,
                            Delivery = currentDelivery,
                            Data = new InvalidDestinationLog
                            {
                                User = _user.Name,
                                Message = $"Invalid Data, Cancelling Delivery: {currentDelivery.DeliveryId}",
                                StartingLocation = startingLocation.Name,
                                DestinationArea = destinationArea.Name
                            }
                        });
                        await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                        // @TODO: create command to remove pending delivery reservations and replace below
                        currentDelivery.Canceled = DateTime.UtcNow;
                        DeliveryCommands.RemoveFromDeliveryQueue(_svtContext, currentDelivery);
                        await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);
                        continue;
                    }

                    var destinationLocation = LocationCommands.GetDeliverableLocationByArea(destinationArea, startingLocation.Area.IsOverflowFor(destinationArea));

                    if (destinationLocation == null)
                    {
                        await LogCommands.CreateLog<NoAvailableLocationsLog>(_svtContext, new DataToLog<NoAvailableLocationsLog>
                        {
                            TrackingId = trackingId,
                            Action = _action.Value,
                            Delivery = currentDelivery,
                            Data = new NoAvailableLocationsLog
                            {
                                User = _user.Name,
                                Message = $"No Available Locations, Delivery: {currentDelivery.DeliveryId}",
                                DestinationArea = destinationArea.Name
                            }
                        });
                        await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                        queueOffset += 1;
                        continue;
                    }

                    destinationLocation.Reserved = true;
                    currentDelivery.Locations.Add(destinationLocation);
                    await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                    try
                    {
                        var multiDestinationRequest = new MultiDestinationRequest
                        {
                            PoolId = pool,
                            GroupId = 1, // @TODO: implement area group data point once we have aethon/lonza layout
                            Timeout = -1,
                            Destinations = new string[] { startingLocation.Name, destinationLocation.Name }
                        };

                        var response = await _aethonApi.SendToMultiDestinations(multiDestinationRequest);

                        if (!response.Success)
                        {
                            var message = !String.IsNullOrEmpty(response.Message) ? response.Message : "Unexpected Aethon Response";
                            var exception = new Exception(message);
                            exception.Data.Add(nameof(response.StatusCode), response.StatusCode);
                            exception.Data.Add(nameof(response.Success), response.Success);
                            exception.Data.Add(nameof(response.Message), response.Message);
                            exception.Data.Add(nameof(response.Content), response.Content);
                            exception.Data.Add("RequestBody", multiDestinationRequest);

                            throw exception;
                        }

                        await LogCommands.CreateLog(_svtContext, new DataToLog<AethonResponse<MultiDestinationResponse>>
                        {
                            TrackingId = trackingId,
                            Action = _action.Value,
                            Delivery = currentDelivery,
                            Data = new AethonResponse<MultiDestinationResponse>
                            {
                                User = _user.Name,
                                StatusCode = response.StatusCode,
                                Message = response.Message,
                                Success = response.Success,
                                Content = response.Content
                            }
                        });
                        await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                        currentDelivery.Jobs.Add(new Job { AethonJobId = response.Content.JobId });
                        DeliveryCommands.RemoveFromDeliveryQueue(_svtContext, currentDelivery);
                        await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);
                        currentCount = await JobCommands.GetActiveJobCountByPool(_svtContext, pool);
                    }
                    catch (Exception e)
                    {
                        ContextCommands.RollbackChanges(_svtContext);

                        if (destinationLocation?.Reserved == true) destinationLocation.Reserved = false;

                        if (currentDelivery?.Locations?.Contains(destinationLocation) == true)
                            currentDelivery.Locations.Remove(destinationLocation);

                        await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                        e.Data.Add(nameof(_environment.ApplicationName), _environment.ApplicationName);
                        e.Data.Add(nameof(_environment.EnvironmentName), _environment.EnvironmentName);

                        await LogCommands.CreateLog<BaseErrorLog>(_svtContext, new DataToLog<BaseErrorLog>
                        {
                            TrackingId = trackingId,
                            Delivery = currentDelivery,
                            Action = _action.Value,
                            Data = new BaseErrorLog
                            {
                                User = _user.Name,
                                Message = $"Aethon Send Request Unsuccessful: {e.Message}",
                                Error = new ErrorLog
                                {
                                    Message = e.Message,
                                    StackTrace = e.StackTrace,
                                    Source = e.Source,
                                    Data = e.Data
                                }
                            }
                        });
                        await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                        queueOffset += 1;
                        continue;
                    }
                }
            }

            return Ok(new { success = true, message = "" });
        }
    }
}