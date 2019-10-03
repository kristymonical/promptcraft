using System.Threading.Tasks;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class DeliveryCommands
    {
        public static async Task<Delivery> CreateNewDelivery(SVTContext context, CreateNewDeliveryRequest request)
        {
            var delivery = new Delivery()
            {
                CartId = request.CartId,
                DeliveryType = request.DeliveryType,
                DestinationAreaId = request.DestinationArea.AreaId,
                OrderId = request.OrderId,
                // @todo remove ternary once able to authenticate requests
                UserId = request.UserId != null ? request.UserId : "lonza/test"
            };

            var entityEntry = await context.Deliveries.AddAsync(delivery);

            return entityEntry.Entity;
        }

        public class CreateNewDeliveryRequest
        {
            public string CartId { get; set; }
            public string OrderId { get; set; }
            public string UserId { get; set; }
            public string DeliveryType { get; set; }
            public Area DestinationArea { get; set; }
            public int? PreviousPrioritizedDeliveryId { get; set; }
            public bool Queued { get; set; }
        }
    }
}