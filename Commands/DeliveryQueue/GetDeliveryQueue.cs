using Microsoft.EntityFrameworkCore;
using System.Linq;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static IQueryable<Delivery> GetDeliveryQueue(SVTContext context, int poolId)
        {
            return context.Deliveries
                .Where(d => d.Completed == null)
                .Where(d => d.Canceled == null)
                .Where(d => d.Locations.Any(loc => loc.Area.PoolId == poolId))
                .Where(d => context.Jobs
                    .All(j => j.DeliveryId != d.DeliveryId)
                );
        }
    }
}
