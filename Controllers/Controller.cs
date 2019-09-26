using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

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

        [HttpGet("cleandelivery")]
        public async Task CreateCleanDeliveryRequest()
        {

            // var sp = $"exec usp_cart_move @User=N'DEMO', @SourceId=3, @DeliveryId=14";
            var sp = $"exec usp_cart_move @DestinationId=18, @DeliveryId=3, @User=N'DEMO', @Reserved=1, @SourceId=13";
            // var sp = $"exec usp_cart_move @AreaId={destinationArea.AreaId}, @CartId=N'{request.CartId}', @OrderId=N'{request.OrderId}', @DeliveryType=N'deliver', @User=N'DEMO', @SourceId={currentLocation.LocationId}, @DestinationId=10, @Reserved=1";
            await _svtContext.Database.ExecuteSqlRawAsync(sp);
        }

        [HttpPost("delivery")]
        public async Task CreateDeliveryRequest([FromBody] CreateDeliveryRequestRequest request)
        {
            var destinationArea = await Commands.GetAreaByNameAsync(_svtContext, request.DestinationArea);
            var currentLocation = await Commands.GetLocationByNameAsync(_svtContext, request.Location);

            // var sp = $"exec usp_cart_move @User=N'DEMO', @SourceId=3, @DeliveryId=14";
            // var sp = $"exec usp_cart_move @DestinationId=10, @DeliveryId=14, @User=N'DEMO'";
            var sp = $"exec usp_cart_move @AreaId={destinationArea.AreaId}, @CartId=N'{request.CartId}', @OrderId=N'{request.OrderId}', @DeliveryType=N'deliver', @User=N'DEMO', @SourceId={currentLocation.LocationId}, @DestinationId=10, @Reserved=1";
            await _svtContext.Database.ExecuteSqlRawAsync(sp);

            // return await _svtContext.Deliveries.OrderByDescending(d => d.DeliveryId).FirstAsync();

            // var destinationLocation = await Commands.GetAvailableAncestorLocation(_svtContext, destinationArea.Name, currentLocation.Area.Name);
            // return $"{destinationLocation.Name}";
            /*
                determine delivery type:
                - destination area is staging -> "staging"
                - start location is suitemal -> "return"
                - else -> "delivery"
            */
            // var reserved = 1; // reserved is always true for delivery request screen
            // hard code user

            // await _svtContext.Database.ExecuteSqlRawAsync($"");
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
    }
}