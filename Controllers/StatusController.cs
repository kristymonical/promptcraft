using System;
using System.Linq;
using System.Threading;
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
    public class StatusController : ControllerBase
    {
        private SVTContext _svtContext;
        private AethonApi _aethonApi;
        private TimeSpan _timeout;

        public StatusController(SVTContext sVTContext, AethonApi aethonApi, int timeout = 15)
        {
            _svtContext = sVTContext;
            _aethonApi = aethonApi;
            _timeout = TimeSpan.FromSeconds(timeout);
        }

        [HttpPut("delivery/status")]
        public async Task<IActionResult> UpdateDeliveryStatus()
        {
            var trackingId = Guid.NewGuid().ToString();
            // @TODO: do we need to load 'User'/'ActionType' entities?
            var user = "StatusService";
            var action = "status";

            await LogCommands.CreateLog(_svtContext, new DataToLog<BaseLogData>
            {
                TrackingId = trackingId,
                Action = action,
                Data = new BaseLogData
                {
                    User = user,
                    Message = $"Running Job Status Service: {DateTime.UtcNow}"
                }
            });
            await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

            var activeJobs = await JobCommands.GetActiveJobs(_svtContext)
                .ToListAsync();

            if (activeJobs.Count > 0)
            {
                var unresolvedJobsDetails = activeJobs.Select(job => _aethonApi.GetJob(job.AethonJobId));
                var jobsDetailsTask = Task.WhenAll(unresolvedJobsDetails);
                try
                {
                    jobsDetailsTask.Wait();
                }
                catch
                {
                    // @TODO: handle unresolved Task errors
                }

                if (jobsDetailsTask.Status != TaskStatus.RanToCompletion)
                {
                    // @TODO: handle Task status issues here
                    return Ok(new { success = false, message = $"Job Details Task Not Resolved Correctly" });
                }

                var responses = jobsDetailsTask.Result
                    .Select(result => result?.FirstOrDefault())
                    .ToList();

                await LogCommands.CreateLog(_svtContext, new DataToLog<AethonJobDetailsLog>
                {
                    TrackingId = trackingId,
                    Action = action,
                    Data = new AethonJobDetailsLog
                    {
                        User = user,
                        Message = $"Aethon Job Details: {DateTime.UtcNow}",
                        JobDetails = responses
                    }
                });
                await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                foreach (var job in activeJobs)
                {
                    var response = responses
                        .Where(resp => resp?.JobId == job.AethonJobId)
                        .FirstOrDefault();

                    // @TODO: replace below logic with expected Aethon adapter unsuccessful response
                    if (response == null)
                    {
                        // @TODO: handle no aethon response for current active job here
                        Console.WriteLine($"\n\nNo Aethon Response For Current Job: {job.JobId} With Aethon Job Id: {job.AethonJobId}");
                        continue;
                    }

                    if (response.End != null)
                    {
                        if (response.State == Aethon.JobStates.Completed && job.Completed == null)
                        {
                            job.Completed = DateTime.UtcNow;
                            job.Canceled = null;
                            job.Expired = null;
                        }
                        else if (response.State == Aethon.JobStates.Canceled && job.Canceled == null)
                        {
                            job.Completed = null;
                            job.Canceled = DateTime.UtcNow;
                            job.Expired = null;
                        }
                        else if (response.State == Aethon.JobStates.Expired && job.Expired == null)
                        {
                            job.Completed = null;
                            job.Canceled = null;
                            job.Expired = DateTime.UtcNow;
                        }
                    }

                    foreach (var responseItinerary in response.Itinerary)
                    {
                        var itinerary = job.Itineraries
                            .Where(i => i.AethonRunId == responseItinerary.RunId)
                            .FirstOrDefault();

                        if (itinerary == null)
                        {
                            itinerary = new Itinerary
                            {
                                AethonRunId = responseItinerary.RunId,
                                Location = job.Delivery.Locations
                                    .Where(loc => loc.LocationId == responseItinerary.DestinationId)
                                    .FirstOrDefault()
                            };

                            job.Itineraries.Add(itinerary);
                        }

                        if (responseItinerary.End != null)
                        {

                            if (responseItinerary.State == Aethon.ItineraryStates.Completed && itinerary.Completed == null)
                            {
                                itinerary.Completed = DateTime.UtcNow;
                                itinerary.TimedOut = null;
                            }
                            // @TODO: change Aethon adapter ItineraryStates enum 'Expired' to 'Timed_Out', then change below line to match
                            else if (responseItinerary.State == Aethon.ItineraryStates.Expired && itinerary.TimedOut == null)
                            {
                                itinerary.Completed = null;
                                itinerary.TimedOut = DateTime.UtcNow;
                            }

                            if (itinerary.Location.Reserved)
                            {
                                itinerary.Location.Reserved = false;
                            }
                            else
                            {
                                itinerary.Location.DeliveryId = null;
                            }

                            if (itinerary.Location.AreaId == job.Delivery.DestinationAreaId)
                            {
                                job.Delivery.Completed = DateTime.UtcNow;
                                job.Delivery.Canceled = null;
                            }
                        }
                    }

                    await _svtContext.SaveChangesAsync();
                }
            }

            return Ok(new { success = true, message = $"Active Job Count: {activeJobs.Count}" });
        }
    }
}