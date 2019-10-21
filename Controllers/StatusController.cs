using System;
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
    public class StatusController : ControllerBase
    {
        private SVTContext _svtContext;
        private AethonApi _aethonApi;

        public StatusController(SVTContext sVTContext, AethonApi aethonApi)
        {
            _svtContext = sVTContext;
            _aethonApi = aethonApi;
        }

        [HttpPut("delivery/status")]
        public async Task<IActionResult> UpdateDeliveryStatus()
        {
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

                /*
                
                    TODOs
                    =====
                    1.  Done - iterate through activeJobs
                    2.  Done - lookup corresponding aethon response by aethon job id
                    3.  Done - iterate through aethon itineraries
                    4.  Done - lookup corresponding activeJob itinerary
                    5.  Done - upsert itinerary
                    6.  Done - update if necessary itinerary completed/timedout fields
                    7.  Done - update if necessary job completed/expired/canceled fields
                    8.  Done - save changes
                    9.  Done - remove aethon response from list
                    10. write to aethon log table
                    11. after active job iteration, iterate through remaining aethon responses and write to error log table/alert                    
                
                */

                if (jobsDetailsTask.Status != TaskStatus.RanToCompletion)
                {
                    // @TODO: handle Task status issues here
                    return Ok(new { success = false, message = $"Job Details Task Not Resolved Correctly" });
                }

                var responses = jobsDetailsTask.Result
                    .Select(result => result?.FirstOrDefault())
                    .ToList();

                foreach (var job in activeJobs)
                {
                    var response = responses
                        .Where(resp => resp?.JobId == job.AethonJobId)
                        .FirstOrDefault();

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

                    // @TODO: write serialized response to aethon log table

                    responses.Remove(response);
                }

                if (responses.Count > 0)
                {
                    // @TODO: handle orphaned responses here
                    Console.WriteLine($"\n\n{responses.Count} Orphaned Aethon Responses Remaining\n\n");
                }
            }

            return Ok(new { success = true, message = $"Active Job Count: {activeJobs.Count}" });
        }
    }
}