using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SVT.Platform.Commands;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Controllers
{
    [Route("api")]
    public class DeliveryController : ControllerBase
    {
        private SVTContext _svtContext;

        public DeliveryController(SVTContext svtContext)
        {
            _svtContext = svtContext;
        }

        // @validation 400 if cartId or cartLocation are not in the correct format
        [HttpPost("delivery/queue")]
        public async Task<IActionResult> CreateDeliveryRequest([FromBody] DeliveryRequests request)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            foreach (var deliveryRequest in request.Deliveries)
            {
                var destinationArea = await AreaCommands.GetAreaByName(_svtContext, deliveryRequest.DestinationArea);
                var currentLocation = await LocationCommands.GetLocationByName(_svtContext, deliveryRequest.Location);

                if (destinationArea == null)
                {
                    return NotFound(new { success = false, message = $"Destination: '{deliveryRequest.DestinationArea}' Not Found." });
                }

                if (currentLocation == null)
                {
                    return NotFound(new { success = false, message = $"Location: '{deliveryRequest.Location}' Not Found." });
                }

                (bool isValidCartLocation, string validationErrorMessage) = await DeliveryCommands.ValidateCurrentCartLocation(_svtContext, currentLocation, deliveryRequest.CartId);

                if (!isValidCartLocation)
                {
                    return Conflict(new { success = false, message = validationErrorMessage });
                }

                var delivery = await DeliveryCommands.CreateNewDelivery(_svtContext, new DeliveryCommands.CreateNewDeliveryRequest
                {
                    CartId = deliveryRequest.CartId,
                    OrderId = deliveryRequest.OrderId,
                    DestinationAreaId = destinationArea.AreaId,
                    UserId = HttpContext.User.Identity.Name,
                    DeliveryType = deliveryRequest.DeliveryType
                });

                delivery.Queued = true;

                var lowestPriorityDelivery = await DeliveryCommands.GetLowestPriorityDelivery(_svtContext, currentLocation.Area.PoolId);

                if (lowestPriorityDelivery != null)
                {
                    delivery.PreviousPrioritizedDeliveryId = lowestPriorityDelivery.DeliveryId;
                }

                delivery.Locations = new List<Location> { currentLocation };

                await _svtContext.SaveChangesAsync();
            }
            
            await transaction.CommitAsync();

            return NoContent();
        }

        public class DeliveryRequest
        {
            public string OrderId { get; set; }
            public string CartId { get; set; }
            public string Location { get; set; }
            public string DestinationArea { get; set; }
            public string DeliveryType { get; set; }
        }

        public class DeliveryRequests
        {
            public List<DeliveryRequest> Deliveries { get; set; }
        }
    }
}