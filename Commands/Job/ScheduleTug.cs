using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Aethon;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class JobCommands
    {
        public static async Task<(bool, int)> ScheduleTug(SVTContext context, AethonApi aethonApi, MultiDestinationRequest request)
        {
            var success = false;
            var jobId = -1;
            var maxRetries = 3;
            var count = 0;
            var multiplierInMillis = 500;
            MultiDestinationResponse response = null;

            while (!success && count <= maxRetries)
            {
                if (count > 0)
                {
                    Console.WriteLine($"\n\nsleeping for: {(count * multiplierInMillis)}\n\n");
                    Thread.Sleep((count * multiplierInMillis));
                }

                try
                {
                    response = await aethonApi.SendToMultiDestinations(request);
                    var log = new AethonSendLog
                    {
                        Log = new AethonSendMultiDestination { Response = response, Request = request }
                    };

                    context.AethonSendLogs.Add(log);
                }
                catch
                {
                    // @TODO: handle errors
                }
                finally
                {
                    count += 1;
                    // @TODO: change aethon adapter MultiDestinationResponse.JobId data type from string to int
                    success = (response?.Code == true && Int32.TryParse(response?.JobId, out jobId));
                }
            }

            return (success, jobId);
        }
    }
}