using System;
using System.Collections.Generic;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class AreaCommands
    {
        public static Area GetIntermediateArea(Area start, Area end)
        {
            Area intermediate = null;
            // below represents the depth-first dependency graph
            var nodes = new Stack<AreaMap>(end.PreviousAreas);
            // below tracks if we've check an areas overflow values already
            var previouslyChecked = new HashSet<Area>();

            while (intermediate == null && nodes.Count > 0)
            {
                var node = nodes.Pop();

                if (node.PreviousAreaId == start.AreaId)
                {
                    intermediate = node.NextArea;
                    continue;
                }
                else if (!previouslyChecked.Contains(node.NextArea))
                {
                    foreach (var overflow in node.NextArea.AreaOverflows)
                    {
                        if (overflow.OverflowAreaId == start.AreaId)
                        {
                            intermediate = node.NextArea;
                            continue;
                        }
                    }
                    previouslyChecked.Add(node.NextArea);
                }

                // no intermediate found above, so load this area's dependencies in the graph
                foreach (var areaMap in node.PreviousArea?.PreviousAreas)
                {
                    nodes.Push(areaMap);
                }
            }

            return intermediate;
        }
    }
}