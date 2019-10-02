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

        public class GetAreasResponse
        {
            public int AreaId { get; set; }
            public string AreaName { get; set; }
        }
    }
}