using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<Delivery> GetLowestPriorityDelivery(SVTContext context)
        {
            var deliveriesInQueue = await DeliveryCommands.GetDeliveryQueue(context).CountAsync();

            if (deliveriesInQueue == 0)
            {
                return null;
            }

            var topDelivery = await DeliveryCommands.GetDeliveryQueue(context)
                .Where(d => d.PreviousPrioritizedDeliveryId == null)
                .FirstAsync();

            var lowestPriorityDelivery = topDelivery;

            while (lowestPriorityDelivery.NextPrioritizedDelivery != null)
            {
                lowestPriorityDelivery = lowestPriorityDelivery.NextPrioritizedDelivery;
            }

            return lowestPriorityDelivery;
        }
    }
}