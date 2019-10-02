using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Types;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class LocationCommands
    {
        public static async Task<Location> GetChildLocationAsync(SVTContext context, SqlHierarchyId parentNode, int areaId)
        {
            var area = await context.Areas
                .Where(area => area.AreaId != areaId)
                .Where(area => area.AreaHierarchies.Any(ah => ah.Node.IsDescendantOf(parentNode).IsTrue))
                .Where(area => area.Locations.Any(loc => loc.DeliveryId == null))
                .FirstAsync();

            return area.Locations
                .Where(loc => loc.DeliveryId == null)
                .First();
        }
    }
}