using System.Collections.Generic;
using System.Threading.Tasks;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<List<Delivery>> GetDeliveryQueueInPriorityOrder(SVTContext context, int poolId)
        {
            var firstQueuedDelivery = await DeliveryCommands.GetHighestPriorityDelivery(context, poolId);

            var current = firstQueuedDelivery;

            var queue = new List<Delivery>();

            while (current != null)
            {
                queue.Add(current);
                current = current.NextPrioritizedDelivery;
            }

            return queue;
        }
    }
}