using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<Delivery> GetHighestPriorityDelivery(SVTContext context, int poolId)
        {
            return await DeliveryCommands.GetDeliveryQueue(context, poolId)
                .Where(d => d.PreviousPrioritizedDeliveryId == null)
                .FirstOrDefaultAsync();
        }
    }
}