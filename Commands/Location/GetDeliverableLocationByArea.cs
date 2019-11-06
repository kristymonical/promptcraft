using System;
using System.Linq;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class LocationCommands
    {
        public static Location GetDeliverableLocationByArea(Area area, bool overflow)
        {
            var primaryLocations = LocationCommands.GetAvailableLocationsByArea(area);

            if (overflow) return primaryLocations.FirstOrDefault();

            return LocationCommands.AggregateAvailableLocationsByArea(area, primaryLocations)
                .FirstOrDefault();
        }
    }
}