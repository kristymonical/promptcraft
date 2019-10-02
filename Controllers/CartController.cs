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
    public class CartController : ControllerBase
    {
        private SVTContext _svtContext;

        public CartController(SVTContext svtContext)
        {
            _svtContext = svtContext;
        }

        [HttpGet("staging")]
        public async Task<IEnumerable<GetStagedCartsResponse>> GetStagedCarts([FromQuery] string deliveryType)
        {
            var stagedCartLocationsQueryable = LocationCommands.GetStagedCartLocations(_svtContext);

            if (deliveryType != null)
            {
                stagedCartLocationsQueryable = stagedCartLocationsQueryable.Where(loc => loc.Delivery.DeliveryType == deliveryType);
            }
            return (await stagedCartLocationsQueryable.ToListAsync())
                .Select(loc => new GetStagedCartsResponse()
                {
                    OrderId = loc.Delivery.OrderId,
                    CartId = loc.Delivery.CartId,
                    StagingLocationId = loc.Name,
                    DeliveryRequestType = loc.Delivery.DeliveryType,
                    DestinationArea = loc.Delivery.DestinationArea.Name
                });
        }

        [HttpPost("move")]
        public async Task MoveCartAsync([FromBody] MoveCartRequest request)
        {
            await DeliveryCommands.MoveCart(_svtContext, request.CartId, request.MalLocationName);
        }

        [HttpGet("cleaninfo")]
        public async Task<GetOrderAndDestinationResponse> GetOrderAndDestination([FromQuery] GetOrderAndDestinationRequest request)
        {
            // var location = _svtContext.Locations
            var activeDelivery = await _svtContext.Deliveries
                .Where(d => d.CartId == request.CartId)
                .Where(d => d.Completed == null)
                .FirstAsync();

            await DeliveryCommands.MoveCart(_svtContext, request.CartId, request.MalLocationName);

            return new GetOrderAndDestinationResponse()
            {
                DestinationAreaName = activeDelivery.DestinationArea.Name,
                OrderId = activeDelivery.OrderId
            };
        }

        public class GetStagedCartsResponse
        {
            public string OrderId { get; set; }
            public string CartId { get; set; }
            public string StagingLocationId { get; set; }
            public string DeliveryRequestType { get; set; }
            public string DestinationArea { get; set; }
        }

        public class MoveCartRequest
        {
            public string MalLocationName { get; set; }
            public string CartId { get; set; }
        }

        public class GetOrderAndDestinationRequest
        {
            public string MalLocationName { get; set; }
            public string CartId { get; set; }
        }

        public class GetOrderAndDestinationResponse
        {
            public string OrderId { get; set; }
            public string DestinationAreaName { get; set; }
        }
    }
}