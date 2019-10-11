using System.Collections.Generic;
using System.Linq;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class LocationCommands
    {
        public static List<Location> AggregateAvailableLocationsByArea(Area area, List<Location> accumulator)
        {
            return area.AreaOverflows
                .OrderBy(areaOverflow => areaOverflow.Priority)
                .Aggregate(accumulator, (accum, areaOverflow) => {
                    accum.AddRange(LocationCommands.GetAvailableLocationsByArea(areaOverflow.OverflowArea));
                    return accum;
                });
        }
    }
}