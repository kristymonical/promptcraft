using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Commands;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;
using System.Text.Json;

namespace SVT.Platform.Controllers
{
    [Route("api")]
    public class LonzaController : ControllerBase
    {
        private SVTContext _svtContext;

        public LonzaController(SVTContext svtContext)
        {
            _svtContext = svtContext;
        }

        [HttpGet("hierarchytest")]
        public async Task CreateCleanDeliveryRequest([FromQuery] string startLocationName)
        {
            var area = await _svtContext.Areas
                .Where(a => a.AreaHierarchies.Any(ah => ah.Node.Equals(0x58)))
                .FirstAsync();
        }

        [HttpPost("delivery")]
        public async Task<int> CreateDeliveryRequest([FromBody] CreateDeliveryRequestRequest request)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();
            // @todo grab real user from IdentityClaims
            // @todo hierarchy
            // @todo priority
            // @validation 400 if cartId is not in cartLocation
            // @validation 400 if cartId or cartLocation are not in the correct format
            // @validation 409 if no locations are available in the destination or staging
            // @delivery-request create command to get queue
            // @delivery-request create command to walk queue to get lowest priority
            var destinationArea = await Commands.GetAreaByNameAsync(_svtContext, request.DestinationArea);
            var currentLocation = await Commands.GetLocationByNameAsync(_svtContext, request.Location);

            var lowestPriorityDelivery = await DeliveryCommands.GetLowestPriorityDelivery(_svtContext, currentLocation.Area.PoolId);

            var deliveryRequest = new DeliveryCommands.CreateNewDeliveryRequest
            {
                CartId = request.CartId,
                OrderId = request.OrderId,
                DestinationArea = destinationArea,
                UserId = "lonza/test", // @hardcoded user id
                PreviousPrioritizedDeliveryId = null,
                DeliveryType = request.DeliveryType,
                Queued = true
            };

            if (lowestPriorityDelivery != null)
            {
                deliveryRequest.PreviousPrioritizedDeliveryId = lowestPriorityDelivery.DeliveryId;
            }

            var delivery = await DeliveryCommands.CreateNewDelivery(_svtContext, deliveryRequest);
            await _svtContext.SaveChangesAsync();

            currentLocation.DeliveryId = delivery.DeliveryId;
            await _svtContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return delivery.DeliveryId;
        }

        [HttpGet("staging")]
        public async Task<IEnumerable<GetStagedCartsResponse>> GetStagedCarts()
        {
            var list = await Commands.GetStagedCartLocationsQuery(_svtContext).ToListAsync();
            return list.Select(loc => new GetStagedCartsResponse()
            {
                OrderId = loc.Delivery.OrderId,
                CartId = loc.Delivery.CartId,
                StagingLocationId = loc.Name,
                DeliveryRequestType = loc.Delivery.DeliveryType,
                DestinationArea = loc.Delivery.DestinationArea.Name
            });
        }

        // [HttpPost("staging")]
        // public async Task CreateStagingRequest()
        // {

        // }

        [HttpGet("areas")]
        public async Task<IEnumerable<GetAreasResponse>> GetAreasAsync()
        {
            var areas = await _svtContext.Areas.ToListAsync();
            return areas.Select(area => new GetAreasResponse()
            {
                AreaId = area.AreaId,
                AreaName = area.Name
            }).OrderBy(x => x.AreaName);
        }

        [HttpPost("move")]
        public async Task MoveCartAsync([FromBody] MoveCartRequest request)
        {
            await Commands.MoveCart(_svtContext, request.CartId, request.MalLocationName);
        }

        [HttpGet("cleaninfo")]
        public async Task<GetOrderAndDestinationResponse> GetOrderAndDestination([FromQuery] GetOrderAndDestinationRequest request)
        {
            // var location = _svtContext.Locations
            var activeDelivery = await _svtContext.Deliveries
                .Where(d => d.CartId == request.CartId)
                .Where(d => d.Completed == null)
                .FirstAsync();

            await Commands.MoveCart(_svtContext, request.CartId, request.MalLocationName);

            return new GetOrderAndDestinationResponse()
            {
                DestinationAreaName = activeDelivery.DestinationArea.Name,
                OrderId = activeDelivery.OrderId
            };
        }

        [HttpGet("delivery-queue")]
        public async Task<IEnumerable<DeliveryResponse>> GetDeliveryQueueByPool([FromQuery]int poolId)
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

            return queue.Select(d => new DeliveryResponse {
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

        [HttpPut("delivery/{deliveryId}/queue/priority")]
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

            if (newParentDelivery != null) {
                currentDelivery.PreviousPrioritizedDeliveryId = newParentDelivery.DeliveryId;
            }

            if(newChildDelivery != null) {
                newChildDelivery.PreviousPrioritizedDeliveryId = currentDelivery.DeliveryId;
            }
            
            await _svtContext.SaveChangesAsync();

            await transaction.CommitAsync();

            return NoContent();
        }

        [HttpPut("delivery/{deliveryId}/queue/priority/top")]
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

        [HttpPut("delivery/{deliveryId}/queue/priority/bottom")]
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

        [HttpPut("delivery/queue/pop")]
        public async Task<IActionResult> PopDeliveryFromQueue([FromQuery]int poolId)
        {
            var top = await DeliveryCommands.PopDeliveryQueue(_svtContext, poolId);

            await _svtContext.SaveChangesAsync();

            return Ok(new DeliveryResponse
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
        public async Task<IActionResult> AppendDeliveryToQueue([FromRoute]int deliveryId, [FromQuery]int poolId)
        {
            var delivery = await DeliveryCommands.GetDeliveryById(_svtContext, deliveryId);

            var lastInQueue = await DeliveryCommands.GetLowestPriorityDelivery(_svtContext, poolId);

            delivery.Queued = true;
            
            if (lastInQueue != null)
            {
                delivery.PreviousPrioritizedDeliveryId = lastInQueue.DeliveryId;
            }

            await _svtContext.SaveChangesAsync();
            
            return NoContent();
        }
    }
}