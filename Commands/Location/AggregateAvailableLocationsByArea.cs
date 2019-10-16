using System;
using System.Collections.Generic;
using System.Linq;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class LocationCommands
    {
        public static List<Location> AggregateAvailableLocationsByArea(Area area, List<Location> accumulator)
        {
            Console.WriteLine($"\n\nArea: {area.AreaId} overflow count: {area.AreaOverflows?.Count}\n\n");
            Console.WriteLine($"\n\nArea: {area.AreaId} overflow for count: {area.AreasOverflowFor?.Count}\n\n");
            
            return area.AreaOverflows
                .OrderBy(areaOverflow => areaOverflow.Priority)
                .Aggregate(accumulator, (accum, areaOverflow) => {
                    accum.AddRange(LocationCommands.GetAvailableLocationsByArea(areaOverflow.OverflowArea));
                    return accum;
                });
        }
    }
}