using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class PoolCommands
    {
        public static IEnumerable<Pool> GetPoolThresholds(SVTContext context)
        {
            return context.Pools.AsEnumerable();
        }
    }
}