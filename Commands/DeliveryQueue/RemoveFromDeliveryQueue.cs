using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static void RemoveFromDeliveryQueue(SVTContext context, Delivery delivery)
        {
            var previousQueuedDelivery = delivery.PreviousPrioritizedDelivery;
            var nextQueuedDelivery = delivery.NextPrioritizedDelivery;

            if (previousQueuedDelivery != null)
            {
                previousQueuedDelivery.NextPrioritizedDelivery = nextQueuedDelivery;
            }
            else if (nextQueuedDelivery != null)
            {
                nextQueuedDelivery.PreviousPrioritizedDelivery = previousQueuedDelivery;
            }

            delivery.Queued = false;
        }
    }
}