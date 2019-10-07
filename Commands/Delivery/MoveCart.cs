using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;
using static SVT.Platform.Controllers.CartController;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task MoveCart(SVTContext context, MoveCartCommand moveCartCommand)
        {
            Delivery delivery;
            
            if (moveCartCommand.CurrentLocation?.Delivery != null)
            {
                delivery = moveCartCommand.CurrentLocation.Delivery;
            } else {
                delivery = await DeliveryCommands.CreateNewDelivery(context, new DeliveryCommands.CreateNewDeliveryRequest{
                    CartId = moveCartCommand.CartId,
                    DeliveryType = "manual",
                    DestinationAreaId = moveCartCommand.DestinationLocation.AreaId,
                    UserId = moveCartCommand.User
                });
                delivery.Completed = DateTime.UtcNow;
            }

            delivery.Locations.Remove(moveCartCommand.CurrentLocation);
            delivery.Locations.Add(moveCartCommand.DestinationLocation);

            moveCartCommand.CurrentLocation.Reserved = false;
            moveCartCommand.DestinationLocation.Reserved = false;

            await context.SaveChangesAsync();
        }
    }
}