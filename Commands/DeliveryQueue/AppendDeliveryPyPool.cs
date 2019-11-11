using System.Threading.Tasks;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<bool> AppendDeliveryByPool(SVTContext context, Delivery delivery, int poolId)
        {
            var lastInQueue = await DeliveryCommands.GetLowestPriorityDelivery(context, poolId);

            delivery.Queued = true;

            if (lastInQueue != null)
            {
                delivery.PreviousPrioritizedDeliveryId = lastInQueue.DeliveryId;
            }

            return true;
        }
    }
}