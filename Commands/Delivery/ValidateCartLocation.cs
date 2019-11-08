using System.Linq;
using System.Threading.Tasks;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<(bool, string)> ValidateCurrentCartLocation(SVTContext context, Location currentLocation, string cartId)
        {
            var currentCartLocation = await LocationCommands.GetCartCurrentLocation(context, cartId);

            if (currentCartLocation != null && currentCartLocation.LocationId != currentLocation?.LocationId)
            {
                return (false, $"Cart: '{cartId}' currently occupies Location: '{currentCartLocation.Name}'.");
            }

            if (currentLocation?.Delivery != null)
            {
                if (currentLocation.Delivery.CartId != cartId)
                {
                    return (false, $"Location: '{currentLocation.Name}' is currently occupied by Cart: '{currentLocation.Delivery.CartId}'.");
                }

                if (currentLocation.Delivery.Completed == null && currentLocation.Delivery.Canceled == null)
                {
                    var status = currentLocation.Delivery.Queued ? "queued" : "active";

                    return (false, $"Location: '{currentLocation.Name}' / Cart: '{cartId}' currently assigned to {status} Delivery:{currentLocation.Delivery.DeliveryId}.");
                }
            }

            return (true, null);
        }

        public static (bool, string) ValidateDestinationCartLocation(Location destinationLocation, string cartId)
        {
            if (destinationLocation?.Delivery != null)
            {
                if (destinationLocation.Delivery.CartId != cartId)
                {
                    return (false, $"Location: '{destinationLocation.Name}' is currently occupied by Cart: '{destinationLocation.Delivery.CartId}'.");
                }
            }

            return (true, null);
        }

        public static (bool, string) ValidateCartLocationItineraries(Location currentLocation, string cartId)
        {
            var activeItineraries = currentLocation?
                .Delivery?
                .Jobs
                .Any(j => j.Itineraries
                    .Any(i => i.LocationId == currentLocation.LocationId
                              && i.Completed == null
                              && i.TimedOut == null));

            if (activeItineraries.HasValue && activeItineraries.Value)
            {
                return (false, $"Cart: {cartId} / Location: {currentLocation.Name} can't be moved due to an existing Tug Job/Itinerary.");
            }

            return (true, null);
        }

        public static (bool, string) ValidateActiveJobCart(string cartId, Delivery delivery)
        {
            var activeJob = delivery?
                .Jobs
                .Where(j => j.Completed == null
                          && j.Canceled == null
                          && j.Expired == null)
                .FirstOrDefault();

            if (activeJob != null)
            {
                return (false, $"Cart: {cartId} can't be moved due to active Job: {activeJob.JobId}");
            }

            return (true, null);
        }

        public static (bool, string) ValidateCartMove(string cartId, Location destinationLocation, Location currentLocation, Delivery activeDelivery = null)
        {
            (bool isValid, string errorMessage) = DeliveryCommands.ValidateCartLocationItineraries(currentLocation, cartId);

            if (!isValid)
            {
                return (isValid, errorMessage);
            }

            (isValid, errorMessage) = DeliveryCommands.ValidateDestinationCartLocation(destinationLocation, cartId);

            if (!isValid)
            {
                return (isValid, errorMessage);
            }

            (isValid, errorMessage) = DeliveryCommands.ValidateActiveJobCart(cartId, activeDelivery);

            if (!isValid)
            {
                return (isValid, errorMessage);
            }

            return (true, null);
        }
    }
}