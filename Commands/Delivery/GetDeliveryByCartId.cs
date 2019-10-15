using Microsoft.EntityFrameworkCore;
using System.Linq;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;
using System.Threading.Tasks;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<Delivery> GetActiveDeliveryByCartId(SVTContext context, string cartId)
        {
            return await context.Deliveries
                .Where(d => d.Completed == null && d.Canceled == null) // filter out completed/canceled deliveries
                .Where(d => d.CartId == cartId)
                .FirstOrDefaultAsync();
        }
    }
}