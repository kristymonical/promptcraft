using System;
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
            var destinationArea = await AreaCommands.GetAreaByName(_svtContext, request.DestinationArea);
            var currentLocation = await LocationCommands.GetLocationByName(_svtContext, request.Location);

            var lowestPriorityDelivery = await DeliveryCommands.GetLowestPriorityDelivery(_svtContext, currentLocation.Area.PoolId);

            Console.WriteLine(lowestPriorityDelivery?.CartId);

            var deliveryRequest = new DeliveryCommands.CreateNewDeliveryRequest
            {
                CartId = request.CartId,
                OrderId = request.OrderId,
                DestinationArea = destinationArea,
                UserId = "ME", // @hardcoded user id
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