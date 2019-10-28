using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SVT.Platform.Commands;
using SVT.Platform.Data;

namespace SVT.Platform.Controllers
{
    /// <summary>
    /// Log Controller Definition
    /// </summary>
    [Route("api")]
    public class LogController : ControllerBase
    {
        private SVTContext _svtContext;
        private IWebHostEnvironment _environment;
        private TimeSpan _timeout;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="svtContext">SQL Server database context</param>
        /// <param name="environment">Application web hosting environment interface</param>
        public LogController(SVTContext svtContext, IWebHostEnvironment environment, int timeout = 15)
        {
            _svtContext = svtContext;
            _environment = environment;
            _timeout = TimeSpan.FromSeconds(timeout);
        }

        /// <summary>
        /// POST route to create new Log
        /// </summary>
        /// <param name="request">Log data to persist</param>
        /// <returns>Task that resolves to IActionResult - Ok (200) on success</returns>
        [HttpPost("log")]
        public async Task<IActionResult> CreateLog([FromBody] LogRequest<WebAppLogRequest> request)
        {
            var userName = HttpContext.User.Identity.Name;

            if (userName == null && _environment.EnvironmentName == "Development")
            {
                userName = "DevAPI";
            }

            await LogCommands.CreateLog(_svtContext, new DataToLog<WebAppLog>
            {
                TrackingId = request.TrackingId,
                Delivery = await DeliveryCommands.GetDeliveryById(_svtContext, request.DeliveryId),
                Action = request.Action,
                Data = new WebAppLog
                {
                    User = userName,
                    Message = request.Data.Message,
                    Method = request.Data.Method,
                    Route = request.Data.Route,
                    StatusCode = request.Data.StatusCode,
                    Success = request.Data.Success
                }
            });
            await _svtContext.SaveChangesAsync(new CancellationTokenSource(_timeout).Token);

            return Ok(new { success = true, message = $"" });
        }

        public class LogRequest<TData>
        {
            public string TrackingId { get; set; } = Guid.NewGuid().ToString();

            public int DeliveryId { get; set; } = -1;

            public string Action { get; set; }

            public TData Data { get; set; }
        }

        public class WebAppLogRequest
        {
            public string Method { get; set; }

            public string Route { get; set; }

            public int StatusCode { get; set; }

            public bool Success { get; set; }

            public string Message { get; set; }
        }
    }
}