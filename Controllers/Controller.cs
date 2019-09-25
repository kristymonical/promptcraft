using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;

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

        [HttpPost("delivery")]
        public async Task CreateDeliveryRequest()
        {

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

        [HttpPost("staging")]
        public async Task CreateStagingRequest()
        {

        }
    }
}