using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<Delivery> PopDeliveryQueue(SVTContext context, int poolId)
        {
            var top = await DeliveryCommands.GetHighestPriorityDelivery(context, poolId);

            top.Queued = false;

            top.NextPrioritizedDelivery.PreviousPrioritizedDeliveryId = null;

            return top;
        }
    }
}