using System;
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
                InsertedBy = request.UserId,
                InsertedOn = DateTime.UtcNow,
                ModifiedBy = "",
                ModifiedOn = DateTime.UnixEpoch, // @hardcoded audit fields
                OrderId = request.OrderId,
                PreviousPrioritizedDeliveryId = request.PreviousPrioritizedDeliveryId,
                UserId = request.UserId
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
        }
    }
}