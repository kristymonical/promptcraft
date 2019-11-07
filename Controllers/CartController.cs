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
    public class CartController : ControllerBase
    {
        private SVTContext _svtContext;

        public CartController(SVTContext svtContext)
        {
            _svtContext = svtContext;
        }

        [HttpGet("staging")]
        public async Task<IActionResult> GetStagedCarts([FromQuery]ByDeliveryType query)
        {
            var stagedCartLocationsQueryable = LocationCommands.GetStagedCartLocations(_svtContext);

            if (!string.IsNullOrWhiteSpace(query.DeliveryType))
            {
                stagedCartLocationsQueryable = stagedCartLocationsQueryable.Where(loc => loc.Delivery.DeliveryType == query.DeliveryType);
            }

            return Ok(new
            {
                success = true,
                message = "",
                data = (await stagedCartLocationsQueryable.ToListAsync())
                    .Select(loc => new GetStagedCartsResponse()
                    {
                        OrderId = loc.Delivery.OrderId,
                        CartId = loc.Delivery.CartId,
                        StagingLocationId = loc.Name,
                        DeliveryRequestType = loc.Delivery.DeliveryType,
                        DestinationArea = loc.Delivery.DestinationArea.Name
                    })
            });
        }

        [HttpPut("move")]
        public async Task<IActionResult> MoveCartAsync([FromBody]MoveCartRequest request)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            var destinationLocation = await LocationCommands.GetLocationByName(_svtContext, request.LocationName);

            if (destinationLocation == null)
            {
                return NotFound(new { success = false, message = $"Location: {request.LocationName} Not Found" });
            }

            var currentCartLocation = await LocationCommands.GetCartCurrentLocation(_svtContext, request.CartId);
            (bool isValidLocation, string errorMessage) = DeliveryCommands.ValidateCartMove(request.CartId, destinationLocation, currentCartLocation);

            if (!isValidLocation)
            {
                return Conflict(new { success = false, message = errorMessage });
            }

            await DeliveryCommands.MoveCart(_svtContext, new MoveCartCommand
            {
                CartId = request.CartId,
                CurrentLocation = currentCartLocation,
                DestinationLocation = destinationLocation,
                User = HttpContext.User.Identity.Name
            });

            await transaction.CommitAsync();
            return Ok(new { success = true, message = "" });
        }

        [HttpGet("cleaninfo")]
        public async Task<IActionResult> GetCleanInfoAsync([FromQuery] CleanInfoRequest request)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            // ensure valid location is passed
            var destinationLocation = await LocationCommands.GetLocationByName(_svtContext, request.MalLocationName);
            if (destinationLocation == null)
            {
                return NotFound(new { success = false, message = $"Location: {request.MalLocationName} Not Found" });
            }

            // ensure cart can be moved to the request location
            var currentCartLocation = await LocationCommands.GetCartCurrentLocation(_svtContext, request.CartId);
            (bool isValidLocation, string errorMessage) = DeliveryCommands.ValidateCartMove(request.CartId, destinationLocation, currentCartLocation);
            if (!isValidLocation)
            {
                return Conflict(new { success = false, message = errorMessage });
            }

            // ensure there is an active delivery for this cart so we can pull destination area and order id
            var delivery = await DeliveryCommands.GetActiveDeliveryByCartId(_svtContext, request.CartId);
            if (delivery == null)
            {
                return NotFound(new { success = false, message = $"No delivery found for Cart: {request.CartId}" });
            }

            await DeliveryCommands.MoveCart(_svtContext, new MoveCartCommand
            {
                CartId = request.CartId,
                CurrentLocation = currentCartLocation,
                DestinationLocation = destinationLocation,
                User = HttpContext.User.Identity.Name
            });

            await transaction.CommitAsync();

            return Ok(new { success = true, message = "", Data = new { DestinationAreaName = delivery.DestinationArea.Name, OrderId = delivery.OrderId } });
        }

        public class CleanInfoRequest
        {
            public string MalLocationName { get; set; }
            public string CartId { get; set; }
        }

        public class ByDeliveryType
        {
            public string DeliveryType { get; set; } = "stage";
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

        public class GetOrderAndDestinationResponse
        {
            public string OrderId { get; set; }
            public string DestinationAreaName { get; set; }
        }
    }
}