using Microsoft.EntityFrameworkCore;
using System.Linq;

using SVT.Platform.Data;
using SVT.Platform.Data.Models;
using System.Threading.Tasks;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<Location> GetCartCurrentLocation(SVTContext context, string cartId)
        {
            var currentDelivery = await context.Deliveries
                .Where(d => d.CartId == cartId)
                .FirstOrDefaultAsync();
            
            if (currentDelivery == null)
            {
                return null;
            }
            
            return currentDelivery.Locations.ToList().Find(loc => !loc.Reserved);
        }
    }
}