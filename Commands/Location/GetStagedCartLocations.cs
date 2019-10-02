using System.Linq;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class LocationCommands
    {
        public static IQueryable<Location> GetStagedCartLocations(SVTContext context)
        {
            return context.Locations
                .Where(loc => loc.DeliveryId != null)
                .Where(loc => loc.Area.AreaType == "stg")
                .Where(loc => loc.Delivery.DeliveryType != "return");
        }
    }
}