using System.Linq;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static IQueryable<Delivery> GetDeliveryQueue(SVTContext context)
        {
            return context.Deliveries
                .Where(d => d.Completed == null)
                .Where(d => d.Canceled == null)
                .Where(d => context.Jobs
                    .All(j => j.DeliveryId != d.DeliveryId)
                );
        }
    }
}