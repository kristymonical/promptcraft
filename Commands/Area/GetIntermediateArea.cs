using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class AreaCommands
    {
        public static Area GetIntermediateArea(Area start, Area end)
        {
            Area intermediate = null;
            var current = end;

            while (intermediate == null && current != null)
            {
                foreach (var areaMap in current.PreviousAreas)
                {
                    if (areaMap.PreviousAreaId == start.AreaId)
                    {
                        intermediate = current;
                        break;
                    }

                    current = areaMap.PreviousArea;
                }
            }

            return intermediate;
        }
    }
}