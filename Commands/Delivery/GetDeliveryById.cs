using Microsoft.EntityFrameworkCore;
using System.Linq;

using SVT.Platform.Data;
using SVT.Platform.Data.Models;
using System.Threading.Tasks;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<Delivery> GetDeliveryById(SVTContext context, int deliveryId)
        {
            return await context.Deliveries
                .Where(d => d.DeliveryId == deliveryId)
                .FirstOrDefaultAsync();
        }
    }
}