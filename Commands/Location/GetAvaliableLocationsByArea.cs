using System;
using System.Collections.Generic;
using System.Linq;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class LocationCommands
    {
        public static List<Location> GetAvailableLocationsByArea(Area area)
        {
            return area.Locations
                .Where(loc => loc.AreaId == area.AreaId)
                .Where(loc => loc.LocationType != "mal" && loc.LocationType != "smal")
                // @TODO: need to figure out better way to do this ^^^^^
                .Where(loc => loc.DeliveryId == null)
                .ToList();
        }
    }
}