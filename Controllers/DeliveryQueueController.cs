using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SVT.Platform.Commands;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Controllers
{
    /// <summary>
    /// Delivery Queue Controller definition
    /// </summary>
    [Route("api")]
    public class DeliveryQueueController : ControllerBase
    {
        private SVTContext _svtContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="svtContext">SQL Server database context</param>
        public DeliveryQueueController(SVTContext svtContext)
        {
            _svtContext = svtContext;
        }

        /// <summary>
        /// GET route to retrieve all "queued" deliveries
        /// </summary>
        /// <param name="poolId">Filter queued deliveries by Robot (TUG) pool</param>
        /// <returns>Task that resolves to IActionResult - Ok (200) on success</returns>
        [HttpGet("delivery-queue")]
        public async Task<IActionResult> GetDeliveryQueueByPool([FromQuery] ByPoolRequest queryParams)
        {
            var queue = await DeliveryCommands.GetDeliveryQueueInPriorityOrder(_svtContext, queryParams.PoolId);

            return Ok(new {
                success = true,
                message = "",
                data = queue.Select(d =>
                    new GetDeliveryQueueByPoolResponse
                    {
                        DeliveryId = d.DeliveryId,
                        UserId = d.UserId,
                        CartId = d.CartId,
                        OrderId = d.OrderId,
                        CurrentLocation = d.Locations.ToList().Find(loc => !loc.Reserved).Name,
                        ReservedLocation = d.Locations.ToList().Find(loc => loc.Reserved)?.Name,
                        DestinationArea = d.DestinationArea.Name,
                        DeliveryType = d.DeliveryType
                    })
            });
        }

        [HttpPut("delivery/{deliveryId}/queue/priority")]
        public async Task<IActionResult> ChangeDeliveryPriority([FromRoute]PriorityRoute route, [FromQuery]PriorityQuery query)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            var currentDelivery = await DeliveryCommands.GetQueuedDeliveryById(_svtContext, route.DeliveryId, query.PoolId);

            if (currentDelivery == null)
            {
                return NotFound(new { success = false, message = $"Delivery: {route.DeliveryId} Not Found"});
            }

            var newParentDelivery = await DeliveryCommands.GetQueuedDeliveryById(_svtContext, query.NewParentDeliveryId, query.PoolId);

            var newChildDelivery = await DeliveryCommands.GetQueuedDeliveryById(_svtContext, query.NewChildDeliveryId, query.PoolId);

            if (newParentDelivery == null && newChildDelivery == null)
            {
                return UnprocessableEntity(new { success = false, message = $"Delivery: {route.DeliveryId} Priority can't be changed as requested" });
            }

            if (currentDelivery.NextPrioritizedDelivery != null)
            {
                currentDelivery.NextPrioritizedDelivery.PreviousPrioritizedDelivery = currentDelivery.PreviousPrioritizedDelivery;
                await _svtContext.SaveChangesAsync(); // @fix this call to save changes fixes a circular dependency error for some reason
            }

            currentDelivery.PreviousPrioritizedDelivery = newParentDelivery;

            newChildDelivery.PreviousPrioritizedDelivery = currentDelivery;

            await _svtContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new { success = true, message = "" });
        }

        [HttpPut("delivery/{deliveryId}/queue/priority/top")]
        public async Task<IActionResult> ChangeDeliveryPriorityTop([FromRoute]PriorityRoute route, [FromQuery]ByPoolRequest query)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            var currentDelivery = await DeliveryCommands.GetQueuedDeliveryById(_svtContext, route.DeliveryId, query.PoolId);

            if (currentDelivery == null)
            {
                return NotFound(new { success = false, message = $"Delivery: {route.DeliveryId} Not Found" });
            }

            var newChildDelivery = await DeliveryCommands.GetHighestPriorityDelivery(_svtContext, query.PoolId);

            if (newChildDelivery == null)
            {
                return UnprocessableEntity(new { success = false, message = $"Delivery: {route.DeliveryId} Priority can't be changed as requested" });
            }

            if (currentDelivery.NextPrioritizedDelivery != null)
            {
                currentDelivery.NextPrioritizedDelivery.PreviousPrioritizedDeliveryId = currentDelivery.PreviousPrioritizedDeliveryId;
            }

            currentDelivery.PreviousPrioritizedDeliveryId = null;

            newChildDelivery.PreviousPrioritizedDeliveryId = currentDelivery.DeliveryId;

            await _svtContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new { success = true, message = "" });
        }

        [HttpPut("delivery/{deliveryId}/queue/priority/bottom")]
        public async Task<IActionResult> ChangeDeliveryPriorityBottom([FromRoute]PriorityRoute route, [FromQuery]ByPoolRequest query)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            var currentDelivery = await DeliveryCommands.GetQueuedDeliveryById(_svtContext, route.DeliveryId, query.PoolId);

            if (currentDelivery == null)
            {
                return NotFound(new { success = false, message = $"Delivery: {route.DeliveryId} Not Found" });
            }

            var newParentDelivery = await DeliveryCommands.GetLowestPriorityDelivery(_svtContext, query.PoolId);

            if (newParentDelivery == null)
            {
                return UnprocessableEntity(new { success = false, message = $"Delivery: {route.DeliveryId} Priority can't be changed as requested" });
            }

            if (currentDelivery.NextPrioritizedDelivery != null)
            {
                currentDelivery.NextPrioritizedDelivery.PreviousPrioritizedDeliveryId = currentDelivery.PreviousPrioritizedDeliveryId;
            }

            currentDelivery.PreviousPrioritizedDeliveryId = newParentDelivery.DeliveryId;

            await _svtContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return Ok(new { success = true, message = "" });
        }

        [HttpPut("delivery/queue/pop")]
        public async Task<IActionResult> PopDeliveryFromQueue([FromQuery]ByPoolRequest query)
        {
            var top = await DeliveryCommands.PopDeliveryQueue(_svtContext, query.PoolId);

            await _svtContext.SaveChangesAsync();

            return Ok(new GetDeliveryQueueByPoolResponse
            {
                DeliveryId = top.DeliveryId,
                UserId = top.UserId,
                CartId = top.CartId,
                OrderId = top.OrderId,
                CurrentLocation = top.Locations.ToList().Find(loc => !loc.Reserved).Name,
                ReservedLocation = top.Locations.ToList().Find(loc => loc.Reserved)?.Name,
                DestinationArea = top.DestinationArea.Name,
                DeliveryType = top.DeliveryType
            });
        }

        [HttpPut("delivery/{deliveryId}/queue/append")]
        public async Task<IActionResult> AppendDeliveryToQueue([FromRoute]PriorityRoute route, [FromQuery]ByPoolRequest query)
        {
            var delivery = await DeliveryCommands.GetDeliveryById(_svtContext, route.DeliveryId);

            var lastInQueue = await DeliveryCommands.GetLowestPriorityDelivery(_svtContext, query.PoolId);

            delivery.Queued = true;

            if (lastInQueue != null)
            {
                delivery.PreviousPrioritizedDeliveryId = lastInQueue.DeliveryId;
            }

            await _svtContext.SaveChangesAsync();

            return Ok(new { success = true, message = "" });
        }

        public class GetDeliveryQueueByPoolResponse
        {
            public int DeliveryId { get; set; }
            public string UserId { get; set; }
            public string CartId { get; set; }
            public string OrderId { get; set; }
            public string CurrentLocation { get; set; }
            public string ReservedLocation { get; set; }
            public string DestinationArea { get; set; }
            public string DeliveryType { get; set; }
        }

        public class ByPoolRequest
        {
            public int PoolId { get; set; }
        }

        public class PriorityRoute
        {
            public int DeliveryId { get; set; }
        }

        public class PriorityQuery
        {
            public int NewChildDeliveryId { get; set; }
            public int NewParentDeliveryId { get; set; }
            public int PoolId { get; set; }
        }
    }
}