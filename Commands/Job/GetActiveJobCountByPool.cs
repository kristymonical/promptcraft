using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;

namespace SVT.Platform.Commands
{
    public partial class JobCommands
    {
        public static async Task<int> GetActiveJobCountByPool(SVTContext context, int poolId)
        {
            return (await JobCommands
                .GetActiveJobs(context)
                .Where(job => job.Delivery
                    .Locations.All(loc => loc.Area.PoolId == poolId)
                )
                .ToListAsync()).Count;
        }
    }
}