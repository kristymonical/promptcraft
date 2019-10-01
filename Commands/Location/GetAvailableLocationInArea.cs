using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class LocationCommands
    {
        public static async Task<Location> GetAvailableLocationInArea(SVTContext context, int areaId)
        {
            return await context.Locations
                .Where(loc => loc.AreaId == areaId && !loc.Reserved && string.IsNullOrWhiteSpace(loc.Delivery.CartId))
                .FirstOrDefaultAsync();
        }
    }
}