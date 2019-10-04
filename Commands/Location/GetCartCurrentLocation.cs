using Microsoft.EntityFrameworkCore;
using System.Linq;

using SVT.Platform.Data;
using SVT.Platform.Data.Models;
using System.Threading.Tasks;

namespace SVT.Platform.Commands
{
    public partial class LocationCommands
    {
        public static async Task<Location> GetCartCurrentLocation(SVTContext context, string cartId)
        {
            var currentDelivery = await context.Deliveries
                .Where(d => d.CartId == cartId)
                .OrderBy(d => d.Completed)
                .FirstOrDefaultAsync();
            
            return currentDelivery?.Locations.ToList().Find(loc => !loc.Reserved);
        }
    }
}