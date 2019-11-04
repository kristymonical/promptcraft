using System.Threading.Tasks;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<bool> PrependDeliveryByPool(SVTContext context, Delivery delivery, int poolId)
        {
            var first = await DeliveryCommands.GetHighestPriorityDelivery(context, poolId);
            delivery.NextPrioritizedDelivery = first;
            delivery.Queued = true;

            return true;
        }
    }
}