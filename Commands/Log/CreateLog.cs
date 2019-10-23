using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            Console.WriteLine($"\n\nTimeout: {timeout.Seconds}\n\n");
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

            await context.SaveChangesAsync(new CancellationTokenSource(timeout).Token);
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
}