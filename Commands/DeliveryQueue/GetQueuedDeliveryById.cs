using Microsoft.EntityFrameworkCore;
using System.Linq;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;
using System.Threading.Tasks;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<Delivery> GetQueuedDeliveryById(
            SVTContext context,
            int deliveryId,
            int poolId)
        {
            return await DeliveryCommands.GetDeliveryQueue(context, poolId)
                .Where(d => d.DeliveryId == deliveryId)
                .FirstOrDefaultAsync();
        }
    }
}