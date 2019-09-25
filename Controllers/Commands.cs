using System;
using System.Linq;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Controllers
{
    public static class Commands
    {
        public static IQueryable<Location> GetStagedCartLocationsQuery(SVTContext context)
        {
            return context.Locations
                .Where(loc => loc.DeliveryId != null)
                .Where(loc => loc.Area.AreaType == "stg")
                .Where(loc => loc.Delivery.DeliveryType != "return");
        }
    }
}