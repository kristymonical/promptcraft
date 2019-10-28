using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aethon;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class LogCommands
    {
        public static async Task CreateLog<TInput>(SVTContext context, DataToLog<TInput> logData, int timeoutInSecs = 15)
        {
            // TODO: look at implementing circuit breaker
            var timeout = TimeSpan.FromSeconds(timeoutInSecs);
            var action = await context.ActionTypes
                .Where(a => a.Value == logData.Action)
                .FirstOrDefaultAsync();

            if (action == null)
            {
                action = await context.ActionTypes
                .Where(a => a.Value == "unknown")
                .FirstOrDefaultAsync();
            }

            await context.Logs.AddAsync(new Log
            {
                TrackingId = String.IsNullOrEmpty(logData.TrackingId) ? (Guid.NewGuid().ToString()) : logData.TrackingId,
                Serialized = JsonConvert.SerializeObject(logData.Data),
                ActionTypeReference = action,
                Delivery = logData.Delivery
            }, new CancellationTokenSource(timeout).Token);

        }
    }

    public class DataToLog<TData>
    {
        public string TrackingId { get; set; }

        public Delivery Delivery { get; set; }

        public string Action { get; set; }

        public TData Data { get; set; }
    }

    public class BaseLogData
    {
        public string User { get; set; }

        public string Message { get; set; }

        public long TimeStamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }

    public class BaseErrorLog : BaseLogData
    {
        public ErrorLog Error { get; set; }
    }

    public class ErrorLog
    {
        public string Message { get; set; }

        public string StackTrace { get; set; }

        public string Source { get; set; }

        public IDictionary Data { get; set; }
    }

    public class NoAvailableLocationsLog : BaseLogData
    {
        public string DestinationArea { get; set; }
    }

    public class InvalidDestinationLog : NoAvailableLocationsLog
    {
        public string StartingLocation { get; set; }
    }

    public class AethonSendLog : BaseLogData
    {
        public bool Success { get; set; }

        public int AethonJobId { get; set; }
    }

    public class AethonJobDetailsLog : BaseLogData
    {
        public List<JobDetailsResponse> JobDetails { get; set; }
    }

    public class WebAppLog : BaseLogData
    {
        public string Method { get; set; }

        public string Route { get; set; }

        public int StatusCode { get; set; }

        public bool Success { get; set; }
    }
}