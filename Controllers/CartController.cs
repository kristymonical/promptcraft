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
        public async Task<IActionResult> MoveCartAsync([FromBody] MoveCartRequest request)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            var currentCartLocation = await LocationCommands.GetCartCurrentLocation(_svtContext, request.CartId);
            var destinationLocation = await LocationCommands.GetLocationByName(_svtContext, request.LocationName);
            (bool isValidLocation, string errorMessage) = DeliveryCommands.ValidateCartMove(request.CartId, destinationLocation, currentCartLocation);

            if (!isValidLocation)
            {
                return Conflict(new { message = errorMessage });
            }

            await DeliveryCommands.MoveCart(_svtContext, new MoveCartCommand{
                CartId = request.CartId,
                CurrentLocation = currentCartLocation,
                DestinationLocation = destinationLocation,
                User = HttpContext.User.Identity.Name
            });

            await transaction.CommitAsync();
            return NoContent();
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
            public string LocationName { get; set; }
            public string CartId { get; set; }
        }

        public class MoveCartCommand
        {
            public string CartId { get; set; }
            public Location CurrentLocation { get; set; }
            public Location DestinationLocation { get; set; }
            public string User { get; set; }
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