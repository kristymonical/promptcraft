using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Commands;
using SVT.Platform.Data;

namespace SVT.Platform.Controllers
{
    [Route("api")]
    public class ScheduleController : ControllerBase
    {
        private SVTContext _svtContext;
        private Dictionary<int, int> _poolThresholds = new Dictionary<int, int>{
            { 1, 2 },
            { 2, 1 },
            { 3, 1 }
        };

        public ScheduleController(SVTContext svtContext)
        {
            _svtContext = svtContext;
        }

        [HttpPost("schedule")]
        public async Task<IActionResult> ScheduleJob()
        {
            foreach (var pool in _poolThresholds.Keys)
            {
                var threshold = _poolThresholds.GetValueOrDefault(pool);
                var deliveryQueueEmpty = false;
                var activeJobCount = await JobCommands.GetActiveJobCountByPool(_svtContext, pool);

                var currentCount = activeJobCount;
                while (!deliveryQueueEmpty && currentCount < threshold)
                {
                    /*
                        @TODO
                        ======
                        1.  _start X-action_
                        2.  _get first delivery in queue (by pool)_
                        3.  calculate destination location
                        4.  reserve destination location
                        5.  create Aethon /send payload
                        6.  call Aethon /send
                        7.  parse Aethon /send response
                        8.  create Job entity
                        9.  create Itirneray entities
                        10. pop delivery off queue (by pool)
                        11. commit/rollback Xaction                    
                    */

                    using var transaction = await _svtContext.Database.BeginTransactionAsync();

                    var currentDelivery = DeliveryCommands.GetHighestPriorityDelivery(_svtContext, pool);

                    if (currentDelivery == null)
                    {
                        deliveryQueueEmpty = true;
                        continue;
                    }

                    // @TODO - replace this increment with a call to 'JobCommands.GetActiveJobCountByPool'
                    currentCount += 1;
                }
            }
            
            return Ok(new { success = true, message = "" });
        }
    }
}