using System.Collections.Generic;
using System.Linq;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class LocationCommands
    {
        public static Location GetDeliverableLocationByArea(Area area)
        {
            var primaryLocations = LocationCommands.GetAvailableLocationsByArea(area);

            return LocationCommands.AggregateAvailableLocationsByArea(area, primaryLocations)
                .FirstOrDefault();
        }
    }
}