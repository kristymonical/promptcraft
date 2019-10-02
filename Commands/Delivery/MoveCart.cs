using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task MoveCart(SVTContext context, string CartId, string MalLocationName)
        {
            var newCartLocation = await context.Locations.Where(loc => loc.Name == MalLocationName).FirstAsync();

            var currentCartLocation = await context.Locations
                .Where(loc => loc.Delivery.CartId == CartId)
                .OrderBy(loc => loc.Delivery.Completed)
                .FirstOrDefaultAsync();

            var activeDelivery = await context.Deliveries
                .Where(d => d.CartId == CartId)
                .Where(d => d.Completed == null)
                .FirstOrDefaultAsync();

            var manualDelivery = context.Deliveries.Add(new Delivery()
            {
                DestinationAreaId = newCartLocation.AreaId,
                CartId = CartId,
                DeliveryType = "manual",
                Completed = DateTime.UtcNow,
                UserId = "DEMO",
                InsertedBy = "sa",
                ModifiedBy = "sa",
                InsertedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            if (currentCartLocation != null && currentCartLocation.LocationId != default(int))
            {
                currentCartLocation.Reserved = false;
                currentCartLocation.DeliveryId = null;
            }

            newCartLocation.DeliveryId = manualDelivery.Entity.DeliveryId;

            await context.SaveChangesAsync();
        }
    }
}