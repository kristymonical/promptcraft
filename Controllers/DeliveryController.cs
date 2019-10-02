using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SVT.Platform.Commands;
using SVT.Platform.Data;

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
        public async Task<IActionResult> CreateDeliveryRequest([FromBody] CreateDeliveryRequestRequest request)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            var destinationArea = await AreaCommands.GetAreaByName(_svtContext, request.DestinationArea);
            var currentLocation = await LocationCommands.GetLocationByName(_svtContext, request.Location);
            var isValidCartLocation = await DeliveryCommands.ValidateCartLocation(_svtContext, currentLocation, request.CartId);

            if (!isValidCartLocation)
            {
                return Conflict();
            }

            var delivery = await DeliveryCommands.CreateNewDelivery(_svtContext, new DeliveryCommands.CreateNewDeliveryRequest
            {
                CartId = request.CartId,
                OrderId = request.OrderId,
                DestinationArea = destinationArea,
                UserId = HttpContext.User.Identity.Name,
                DeliveryType = request.DeliveryType
            });
            
            delivery.Queued = true;
            
            var lowestPriorityDelivery = await DeliveryCommands.GetLowestPriorityDelivery(_svtContext, currentLocation.Area.PoolId);

            if (lowestPriorityDelivery != null)
            {
                delivery.PreviousPrioritizedDeliveryId = lowestPriorityDelivery.DeliveryId;
            }

            await _svtContext.SaveChangesAsync();

            currentLocation.DeliveryId = delivery.DeliveryId;
            await _svtContext.SaveChangesAsync();

            await transaction.CommitAsync();
            return Ok(new { DeliveryId = delivery.DeliveryId });
        }

        public class CreateDeliveryRequestRequest
        {
            public string OrderId { get; set; }
            public string CartId { get; set; }
            public string Location { get; set; }
            public string DestinationArea { get; set; }
            public string DeliveryType { get; set; }
        }
    }
}