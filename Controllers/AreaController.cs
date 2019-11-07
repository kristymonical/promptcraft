using System;
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
    public class AreaController : ControllerBase
    {
        private SVTContext _svtContext;

        public AreaController(SVTContext svtContext)
        {
            _svtContext = svtContext;
        }

        [HttpGet("areas")]
        public async Task<IActionResult> GetAreas()
        {
            var areas = await _svtContext.Areas.ToListAsync();
            var mappedAreas = areas.Select(area => new GetAreasResponse()
            {
                AreaId = area.AreaId,
                AreaName = area.Name
            }).OrderBy(x => x.AreaName);

            return Ok(new { success = true, message = "", data = mappedAreas });
        }

        [HttpGet("areas/destination")]
        public async Task<IActionResult> GetDeliveryDestinationAreas([FromQuery]ByLocationDeliveryType query)
        {
            var currentLocation = await LocationCommands.GetLocationByName(_svtContext, query.LocationName);

            if (currentLocation == null)
            {
                return NotFound(new { success = false, message = $"Location: '{query.LocationName}' Not Found." });
            }

            Area.GraphDirection direction;
            Func<Area, Area, bool> predicate;

            switch (query.DeliveryType)
            {
                case "deliver":
                    direction = Area.GraphDirection.descendants;
                    predicate = (start, current) => (start != current && current?.AreaType == "smal");
                    break;
                case "return":
                    direction = Area.GraphDirection.ancestors;
                    predicate = (start, current) => (start != current && (current?.AreaType == "fpa" || current?.AreaType == "cw"));
                    break;
                case "stage":
                    direction = Area.GraphDirection.descendants;
                    predicate = (start, current) => (start != current && current?.AreaType == "stg");
                    break;
                default:
                    direction = Area.GraphDirection.descendants;
                    predicate = (start, current) => (start != current);
                    break;
            }

            var destinations = currentLocation.Area
                .GetAreasBy(direction, predicate)
                .Select(a => new GetAreasResponse { AreaId = a.AreaId, AreaName = a.Name });

            return Ok(new { success = true, message = "", data = destinations });
        }

        public class GetAreasResponse
        {
            public int AreaId { get; set; }

            public string AreaName { get; set; }
        }

        public class ByLocationDeliveryType
        {
            public string LocationName { get; set; }

            public string DeliveryType { get; set; } = "deliver";
        }
    }
}