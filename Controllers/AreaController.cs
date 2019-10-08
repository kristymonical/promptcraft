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
    public class AreaController : ControllerBase
    {
        private SVTContext _svtContext;

        public AreaController(SVTContext svtContext)
        {
            _svtContext = svtContext;
        }

        [HttpGet("areas")]
        public async Task<IEnumerable<GetAreasResponse>> GetAreas()
        {
            var areas = await _svtContext.Areas.ToListAsync();
            return areas.Select(area => new GetAreasResponse()
            {
                AreaId = area.AreaId,
                AreaName = area.Name
            }).OrderBy(x => x.AreaName);
        }

        [HttpGet("areas/destination")]
        public async Task<IActionResult> GetDestinationAreas([FromQuery] string locationName)
        {
            var currentLocation = await LocationCommands.GetLocationByName(_svtContext, locationName);
            
            if (currentLocation == null)
            {
                return NotFound(new { success = false, message = $"Location: '{locationName}' Not Found." });
            }
            
            var destinations = new List<GetAreasResponse>();
            var nodes = new List<Area>();
            currentLocation.Area.GetLeafNodes(nodes);

            foreach (var area in nodes)
            {
                destinations.Add(new GetAreasResponse{
                    AreaId = area.AreaId,
                    AreaName = area.Name
                });
            }

            return Ok(new { success = true, message = "", data = destinations });
        }

        public class GetAreasResponse
        {
            public int AreaId { get; set; }
            public string AreaName { get; set; }
        }
    }
}