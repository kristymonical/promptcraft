using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aethon;
using Microsoft.AspNetCore.Hosting;
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
        private IWebHostEnvironment _environment;
        private User _user;
        private ActionType _action;

        public StatusController(SVTContext sVTContext, AethonApi aethonApi, IWebHostEnvironment environment, int timeout = 15)
        {
            _svtContext = sVTContext;
            _aethonApi = aethonApi;
            _timeout = TimeSpan.FromSeconds(timeout);
            _environment = environment;
            _user = UserCommands.GetUserByName(_svtContext, "StatusService");
            _action = ActionCommands.GetActionByValue(_svtContext, "status");
        }

        [HttpPut("delivery/status")]
        public async Task<IActionResult> UpdateDeliveryStatus()
        {
            var trackingId = Guid.NewGuid().ToString();

            await LogCommands.CreateLog(_svtContext, new DataToLog<BaseLogData>
            {
                TrackingId = trackingId,
                Action = _action.Value,
                Data = new BaseLogData
                {
                    User = _user.Name,
                    Message = $"Running Job Status Service: {DateTime.UtcNow}"
                }
            });
            await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

            var activeJobs = await JobCommands.GetActiveJobs(_svtContext)
                .ToListAsync();

            if (activeJobs.Count > 0)
            {
                foreach (var job in activeJobs)
                {
                    try
                    {
                        var response = await _aethonApi.GetJob(job.AethonJobId);

                        if (!response.Success)
                        {
                            var message = !String.IsNullOrEmpty(response.Message) ? response.Message : "Unexpected Aethon Response";
                            var exception = new Exception(message);
                            exception.Data.Add(nameof(response.StatusCode), response.StatusCode);
                            exception.Data.Add(nameof(response.Success), response.Success);
                            exception.Data.Add(nameof(response.Message), response.Message);
                            exception.Data.Add(nameof(response.Content), response.Content);

                            throw exception;
                        }

                        await LogCommands.CreateLog(_svtContext, new DataToLog<AethonResponse<List<JobDetailsResponse>>>
                        {
                            TrackingId = trackingId,
                            Action = _action.Value,
                            Data = new AethonResponse<List<JobDetailsResponse>>
                            {
                                User = _user.Name,
                                StatusCode = response.StatusCode,
                                Message = response.Message,
                                Success = response.Success,
                                Content = response.Content
                            }
                        });
                        await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                        var aethonJobDetails = response.Content?.FirstOrDefault();

                        if (aethonJobDetails == null)
                        {
                            // @TODO: cancel job/itineraries

                            await LogCommands.CreateLog(_svtContext, new DataToLog<BaseLogData>
                            {
                                TrackingId = trackingId,
                                Action = _action.Value,
                                Delivery = job.Delivery,
                                Data = new BaseLogData
                                {
                                    User = _user.Name,
                                    Message = $"No Aethon Job Exists For Toolkit Job: {job.JobId}"
                                }
                            });
                            await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);
                            continue;
                        }

                        if (aethonJobDetails.End != null)
                        {
                            if (aethonJobDetails.State == Aethon.JobStates.Completed && job.Completed == null)
                            {
                                job.Completed = DateTime.UtcNow;
                                job.Canceled = null;
                                job.Expired = null;
                            }
                            else if (aethonJobDetails.State == Aethon.JobStates.Canceled && job.Canceled == null)
                            {
                                job.Completed = null;
                                job.Canceled = DateTime.UtcNow;
                                job.Expired = null;
                            }
                            else if (aethonJobDetails.State == Aethon.JobStates.Expired && job.Expired == null)
                            {
                                job.Completed = null;
                                job.Canceled = null;
                                job.Expired = DateTime.UtcNow;
                            }
                        }
                        await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

                        foreach (var aethonJobItinerary in aethonJobDetails.Itinerary)
                        {
                            var itinerary = job.Itineraries
                                .Where(i => i.AethonRunId == aethonJobItinerary.RunId)
                                .FirstOrDefault();

                            // create itinerary if it doesn't exist
                            if (itinerary == null)
                            {
                                itinerary = new Itinerary
                                {
                                    AethonRunId = aethonJobItinerary.RunId,
                                    Location = job.Delivery.Locations
                                        .Where(loc => loc.LocationId == aethonJobItinerary.DestinationId)
                                        .FirstOrDefault()
                                };

                                job.Itineraries.Add(itinerary);
                            }

                            // itinerary leg complete
                            if (aethonJobItinerary.End != null)
                            {
                                if (aethonJobItinerary.State == Aethon.ItineraryStates.Completed && itinerary.Completed == null)
                                {
                                    itinerary.Completed = DateTime.UtcNow;
                                    itinerary.TimedOut = null;

                                    // destination itinerary leg
                                    if (itinerary.Location.Reserved)
                                    {
                                        // mark the delivery complete when at final destination
                                        if (itinerary.Location.AreaId == job.Delivery.DestinationAreaId)
                                        {
                                            job.Delivery.Completed = DateTime.UtcNow;
                                            job.Delivery.Canceled = null;
                                        }
                                        // re-queue the delivery if current area is overflow for next destination area
                                        else
                                        {
                                            var itinerayArea = itinerary.Location.Area;
                                            var deliveryDestinationArea = job.Delivery.DestinationArea;

                                            var nextDestinationArea = deliveryDestinationArea
                                                .GetAreasBy(Area.GraphDirection.ancestors, (finalNode, current) =>
                                                    itinerayArea != current &&
                                                    (current.GetAncestors().Contains(itinerayArea) || current.GetOverflowAreas().Contains(itinerayArea)) &&
                                                    current.Pool == itinerayArea.Pool)
                                                .FirstOrDefault();

                                            if (itinerayArea.IsOverflowFor(nextDestinationArea))
                                            {
                                                await DeliveryCommands.PrependDeliveryByPool(_svtContext, job.Delivery, itinerary.Location.Area.PoolId);
                                            }
                                        }
                                    }
                                    // pickup itinerary leg
                                    else
                                    {
                                        itinerary.Location.DeliveryId = null;
                                    }
                                }
                                else if (aethonJobItinerary.State == Aethon.ItineraryStates.Timed_Out && itinerary.TimedOut == null)
                                {
                                    itinerary.Completed = null;
                                    itinerary.TimedOut = DateTime.UtcNow;
                                }

                                itinerary.Location.Reserved = false;
                            }

                            await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);
                        }
                    }
                    catch (Exception e)
                    {
                        ContextCommands.RollbackChanges(_svtContext);

                        e.Data.Add(nameof(_environment.ApplicationName), _environment.ApplicationName);
                        e.Data.Add(nameof(_environment.EnvironmentName), _environment.EnvironmentName);

                        await LogCommands.CreateLog<BaseErrorLog>(_svtContext, new DataToLog<BaseErrorLog>
                        {
                            TrackingId = trackingId,
                            Action = _action.Value,
                            Data = new BaseErrorLog
                            {
                                User = _user.Name,
                                Message = e.Message,
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

                        return Ok(new { success = false, message = $"Status Service Run Unsuccessful: '{e.Message}'" });
                    }
                }
            }

            return Ok(new { success = true, message = $"Active Job Count: {activeJobs.Count}" });
        }
    }
}