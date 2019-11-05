using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SVT.Platform.Services
{
    public class SchedulingService : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private HttpClient _client;

        public SchedulingService(HttpClient client)
        {
            _client = client;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new System.Timers.Timer(5000)
            {
                AutoReset = false,
                Enabled = true
            };

            _timer.Elapsed += DoWork;

            return Task.CompletedTask;
        }

        private void DoWork(object state, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                _client.PostAsync("api/delivery/schedule", new StringContent("")).Wait();
            }
            catch { }
            finally
            {
                _timer.Start();
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            this.Dispose();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}