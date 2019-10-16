using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Commands;
using SVT.Platform.Data;

namespace SVT.Platform.Controllers
{
    [Route("api")]
    public class StatusController : ControllerBase
    {
        private SVTContext _svtContext;

        public StatusController(SVTContext sVTContext)
        {
            _svtContext = sVTContext;
        }

        [HttpPut("delivery/status")]
        public async Task<IActionResult> UpdateDeliveryStatus()
        {
            var activeJobs = await JobCommands.GetActiveJobs(_svtContext)
                .ToListAsync();
            
            return Ok(new { success = true, message = "" });
        }
    }
}