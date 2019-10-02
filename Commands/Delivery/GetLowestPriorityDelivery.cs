using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<Delivery> GetLowestPriorityDelivery(SVTContext context, int poolId)
        {
            // var deliveriesInQueue = await DeliveryCommands.GetDeliveryQueue(context, poolId).CountAsync();

            // if (deliveriesInQueue == 0)
            // {
            //     return null;
            // }

            var topDelivery = await DeliveryCommands.GetHighestPriorityDelivery(context, poolId);

            var lowestPriorityDelivery = topDelivery;

            while (lowestPriorityDelivery?.NextPrioritizedDelivery != null)
            {
                lowestPriorityDelivery = lowestPriorityDelivery.NextPrioritizedDelivery;
            }

            return lowestPriorityDelivery;
        }
    }
}