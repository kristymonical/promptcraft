using System;
using System.Threading;
using System.Threading.Tasks;
using Aethon;

namespace SVT.Platform.Commands
{
    public partial class JobCommands
    {
        public static async Task<(bool, int)> ScheduleTug(AethonApi aethonApi, MultiDestinationRequest request)
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