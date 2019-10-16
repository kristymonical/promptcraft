using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aethon;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            { 1, 10 },
            { 2, 1 },
            { 3, 1 }
        };

        public ScheduleController(SVTContext svtContext, AethonApi aethonApi)
        {
            _svtContext = svtContext;
            _aethonApi = aethonApi;
        }

        [HttpPost("delivery/schedule")]
        public async Task<IActionResult> ScheduleJob()
        {
            // @TODO: remove between below tags when Aethon adapter/connector call is implemented
            // @from-here
            var rnd = new Random((int)((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds());
            var idCache = new List<int>();
            int _getId()
            {
                int newId = rnd.Next();
                while (idCache.Contains(newId))
                {
                    newId = rnd.Next();
                }
                idCache.Add(newId);
                return newId;
            }
            // @to-here

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
                    Console.WriteLine($"\n\nOffset: {queueOffset}\n\n");

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
                        // @TODO: write to ErrorLog table here
                        // @TODO: implement possible bad data alerting here
                        currentDelivery.Canceled = DateTime.UtcNow;
                        await DeliveryCommands.PopDeliveryQueue(_svtContext, pool);
                        await _svtContext.SaveChangesAsync();
                        continue;
                    }

                    var destinationLocation = LocationCommands.GetDeliverableLocationByArea(destinationArea);

                    if (destinationLocation == null)
                    {
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
                        // @TODO: log unexpected Aethon error to ErrorLog table here
                        destinationLocation.Reserved = false;
                        currentDelivery.Locations.Remove(destinationLocation);
                        await _svtContext.SaveChangesAsync();
                        queueOffset += 1;
                        continue;
                    }

                    // @TODO: Write to AethonLog table here
                    // @TODO: Aethon job status call(s) (for Itineraries) go here

                    currentDelivery.Jobs.Add(new Job
                    {
                        AethonJobId = aethonJobId,
                        Itineraries = new List<Itinerary> {
                            new Itinerary { AethonRunId = _getId(), Location = startingLocation },
                            new Itinerary { AethonRunId = _getId(), Location = destinationLocation }
                        }
                    });

                    await DeliveryCommands.PopDeliveryQueue(_svtContext, pool);
                    await _svtContext.SaveChangesAsync();

                    currentCount = await JobCommands.GetActiveJobCountByPool(_svtContext, pool);
                }
            }

            return Ok(new { success = true, message = "" });
        }
    }
}