using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Commands;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Controllers
{
    [Route("api")]
    public class DeliveryQueueController : ControllerBase
    {
        private SVTContext _svtContext;

        public DeliveryQueueController(SVTContext svtContext)
        {
            _svtContext = svtContext;
        }

        [HttpGet("delivery-queue")]
        public async Task<IEnumerable<GetDeliveryQueueByPoolResponse>> GetDeliveryQueueByPool([FromQuery]int poolId)
        {
            Console.WriteLine($"\n\nPool id: {poolId}\n\n");

            var firstQueuedDelivery = await DeliveryCommands.GetDeliveryQueue(_svtContext, poolId)
                .Where(d => d.PreviousPrioritizedDeliveryId == null)
                .FirstAsync();

            var current = firstQueuedDelivery;

            var queue = new List<Delivery>();

            while (current != null)
            {
                queue.Add(current);
                current = current.NextPrioritizedDelivery;
            }

            return queue.Select(d => new GetDeliveryQueueByPoolResponse
            {
                DeliveryId = d.DeliveryId,
                UserId = d.UserId,
                CartId = d.CartId,
                OrderId = d.OrderId,
                CurrentLocation = d.Locations.ToList().Find(loc => !loc.Reserved).Name,
                ReservedLocation = d.Locations.ToList().Find(loc => loc.Reserved)?.Name,
                DestinationArea = d.DestinationArea.Name,
                DeliveryType = d.DeliveryType
            });
        }

        [HttpPut("delivery/{deliveryId}/change-priority")]
        public async Task<IActionResult> ChangeDeliveryPriority(
            [FromRoute]int deliveryId,
            [FromQuery]int newParentDeliveryId,
            [FromQuery]int newChildDeliveryId,
            [FromQuery]int poolId)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            var currentDelivery = await DeliveryCommands.GetQueuedDeliveryById(_svtContext, deliveryId, poolId);

            if (currentDelivery == null)
            {
                return NotFound();
            }

            var newParentDelivery = await DeliveryCommands.GetQueuedDeliveryById(_svtContext, newParentDeliveryId, poolId);

            var newChildDelivery = await DeliveryCommands.GetQueuedDeliveryById(_svtContext, newChildDeliveryId, poolId);

            if (newParentDelivery == null && newChildDelivery == null)
            {
                return UnprocessableEntity();
            }

            if (currentDelivery.NextPrioritizedDelivery != null)
            {
                currentDelivery.NextPrioritizedDelivery.PreviousPrioritizedDeliveryId = currentDelivery.PreviousPrioritizedDeliveryId;
            }

            if (newParentDelivery != null)
            {
                currentDelivery.PreviousPrioritizedDeliveryId = newParentDelivery.DeliveryId;
            }

            if (newChildDelivery != null)
            {
                newChildDelivery.PreviousPrioritizedDeliveryId = currentDelivery.DeliveryId;
            }

            await _svtContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return NoContent();
        }

        [HttpPut("delivery/{deliveryId}/change-priority/top")]
        public async Task<IActionResult> ChangeDeliveryPriorityTop([FromRoute]int deliveryId, [FromQuery]int poolId)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            var currentDelivery = await DeliveryCommands.GetQueuedDeliveryById(_svtContext, deliveryId, poolId);

            if (currentDelivery == null)
            {
                return NotFound();
            }

            var newChildDelivery = await DeliveryCommands.GetHighestPriorityDelivery(_svtContext, poolId);

            if (newChildDelivery == null)
            {
                return UnprocessableEntity();
            }

            if (currentDelivery.NextPrioritizedDelivery != null)
            {
                currentDelivery.NextPrioritizedDelivery.PreviousPrioritizedDeliveryId = currentDelivery.PreviousPrioritizedDeliveryId;
            }

            currentDelivery.PreviousPrioritizedDeliveryId = null;

            newChildDelivery.PreviousPrioritizedDeliveryId = currentDelivery.DeliveryId;

            await _svtContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return NoContent();
        }

        [HttpPut("delivery/{deliveryId}/change-priority/bottom")]
        public async Task<IActionResult> ChangeDeliveryPriorityBottom([FromRoute]int deliveryId, [FromQuery]int poolId)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            var currentDelivery = await DeliveryCommands.GetQueuedDeliveryById(_svtContext, deliveryId, poolId);

            if (currentDelivery == null)
            {
                return NotFound();
            }

            var newParentDelivery = await DeliveryCommands.GetLowestPriorityDelivery(_svtContext, poolId);

            if (newParentDelivery == null)
            {
                return UnprocessableEntity();
            }

            if (currentDelivery.NextPrioritizedDelivery != null)
            {
                currentDelivery.NextPrioritizedDelivery.PreviousPrioritizedDeliveryId = currentDelivery.PreviousPrioritizedDeliveryId;
            }

            currentDelivery.PreviousPrioritizedDeliveryId = newParentDelivery.DeliveryId;

            await _svtContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return NoContent();
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
    }
}