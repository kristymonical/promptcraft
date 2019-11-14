using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class LocationCommands
    {
        public static async Task<IEnumerable<Location>> GetStagedCartLocations(SVTContext context)
        {
            return (await context.Locations
                .Where(loc => loc.Area.AreaType == "stg")
                .Where(loc => loc.DeliveryId != null)
                .Where(loc => loc.Delivery.Canceled == null)
                .ToListAsync())
                .Where(loc => !loc.Delivery.HasActiveJobs());
        }
    }
}