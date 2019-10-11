using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        // @TODO: remove once configuration is implemented
        private Dictionary<int, int> _poolThresholds = new Dictionary<int, int>{
            { 1, 3 },
            { 2, 1 },
            { 3, 1 }
        };

        public ScheduleController(SVTContext svtContext)
        {
            _svtContext = svtContext;
        }

        [HttpPost("schedule")]
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

            // @TODO: figure out how to implement configurable pool threshold values
            foreach (var pool in _poolThresholds.Keys)
            {
                var threshold = _poolThresholds.GetValueOrDefault(pool);
                var deliveryQueueEmpty = false;
                var activeJobCount = await JobCommands.GetActiveJobCountByPool(_svtContext, pool);

                var currentCount = activeJobCount;

                while (!deliveryQueueEmpty && currentCount < threshold)
                {
                    /*
                        @TODO
                        =====
                        1.  _start X-action_
                        2.  _pop delivery off queue (by pool)_
                        3.  _calculate/reserve destination location_
                        4.  create Aethon /send payload - stubbed for now
                        5.  call Aethon /send - stubbed for now
                        6.  parse Aethon /send response - stubbed for now
                        7.  _create Job entity_
                        8.  _create Itinerary entities_
                        9.  _commit/rollback Xaction_
                    */

                    using var transaction = await _svtContext.Database.BeginTransactionAsync();

                    var currentDelivery = await DeliveryCommands.PopDeliveryQueue(_svtContext, pool);

                    if (currentDelivery == null)
                    {
                        deliveryQueueEmpty = true;
                        await transaction.CommitAsync();
                        continue;
                    }

                    var startingLocation = currentDelivery.Locations.FirstOrDefault();
                    var destinationArea = AreaCommands.GetIntermediateArea(startingLocation.Area, currentDelivery.DestinationArea);

                    if (startingLocation == null || destinationArea == null)
                    {
                        // @TODO: write to ErrorLog table here
                        currentDelivery.Canceled = DateTime.UtcNow;
                        await _svtContext.SaveChangesAsync();
                        await transaction.CommitAsync();
                        continue;
                    }

                    var destinationLocation = LocationCommands.GetDeliverableLocationByArea(destinationArea);

                    destinationLocation.Reserved = true;
                    currentDelivery.Locations.Add(destinationLocation);

                    // @TODO: Aethon adapter/connector call(s) go here
                    // @TODO: Write to AethonLog table here

                    currentDelivery.Jobs.Add(new Job
                    {
                        AethonJobId = _getId(),
                        Itineraries = new List<Itinerary> {
                            new Itinerary { AethonRunId = _getId(), Location = startingLocation },
                            new Itinerary { AethonRunId = _getId(), Location = destinationLocation }
                        }
                    });

                    await _svtContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    currentCount = await JobCommands.GetActiveJobCountByPool(_svtContext, pool);
                }
            }

            return Ok(new { success = true, message = "" });
        }
    }
}