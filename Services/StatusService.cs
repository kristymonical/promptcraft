using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SVT.Platform.Services
{
    public class StatusService : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private HttpClient _client;

        public StatusService(HttpClient client)
        {
            _client = client;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new System.Timers.Timer(5000)
            {
                AutoReset = false,
                Enabled = true
            };

            _timer.Elapsed += DoWork;

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.Dispose();
            return Task.CompletedTask;
        }

        public void DoWork(object state, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _client.PutAsync("api/delivery/status", new StringContent(""));
            }
            catch { }
            finally
            {
                _timer.Start();
            }
        }
    }
}