using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;
using static SVT.Platform.Controllers.CartController;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task MoveCart(SVTContext context, MoveCartCommand moveCartCommand)
        {
            // don't do anything if cart is already in the destination location
            if (moveCartCommand.CurrentLocation == moveCartCommand.DestinationLocation) return;

            Delivery delivery;

            // use existing delivery if it exists
            if (moveCartCommand.CurrentLocation?.Delivery != null)
            {
                delivery = moveCartCommand.CurrentLocation.Delivery;
            }
            else
            {
                // otherwise create a new completed manual delivery
                delivery = await DeliveryCommands.CreateNewDelivery(context, new DeliveryCommands.CreateNewDeliveryRequest
                {
                    CartId = moveCartCommand.CartId,
                    DeliveryType = "manual",
                    DestinationAreaId = moveCartCommand.DestinationLocation.AreaId,
                    UserId = moveCartCommand.User
                });
                delivery.Completed = DateTime.UtcNow;
                delivery.Locations = new List<Location>();
            }

            delivery.Locations.Add(moveCartCommand.DestinationLocation);
            moveCartCommand.DestinationLocation.Reserved = false;

            if (moveCartCommand.CurrentLocation != null)
            {
                delivery.Locations.Remove(moveCartCommand.CurrentLocation);
                moveCartCommand.CurrentLocation.Reserved = false;
            }

            await context.SaveChangesAsync();
        }
    }
}