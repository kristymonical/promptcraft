using System.Threading.Tasks;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<bool> ValidateCartLocation(SVTContext context, Location location, string cartId)
        {
            var currentCartLocation = await DeliveryCommands.GetCartCurrentLocation(context, cartId);

            // cart currently registered to another location
            if (currentCartLocation != null && currentCartLocation.LocationId != location?.LocationId)
            {
                return false;
            }

            // current location has a different cart registered and/or an active delivery assigned
            if (location?.DeliveryId != null)
            {
                var currentLocationDelivery = await DeliveryCommands.GetDeliveryById(context, location.DeliveryId.Value);

                if (currentLocationDelivery.CartId != cartId || (currentLocationDelivery.Completed == null && currentLocationDelivery.Canceled == null))
                {
                    return false;
                }
            }

            return true;
        }
    }
}