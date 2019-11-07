using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SVT.Platform.Commands;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;
using System.Threading.Tasks;

namespace SVT.Platform.Controllers
{
    /// <summary>
    /// Delivery Controller Definition
    /// </summary>
    [Route("api")]
    public class DeliveryController : ControllerBase
    {
        private SVTContext _svtContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="svtContext">SQL Server database context</param>
        public DeliveryController(SVTContext svtContext)
        {
            _svtContext = svtContext;
        }

        /// <summary>
        /// POST route to create/queue a new Delivery
        /// </summary>
        /// <param name="request">List of Delivery instances</param>
        /// <returns>Task that resovles IActionResult - Ok (200) on success</returns>
        [HttpPost("delivery/queue")]
        public async Task<IActionResult> CreateDeliveryRequest([FromBody] DeliveryRequests request)
        {
            using var transaction = await _svtContext.Database.BeginTransactionAsync();

            var messagePrefix = "Delivery Request NOT Created.";

            foreach (var deliveryRequest in request.Deliveries)
            {
                var destinationArea = await AreaCommands.GetAreaByName(_svtContext, deliveryRequest.DestinationArea);
                var currentLocation = await LocationCommands.GetLocationByName(_svtContext, deliveryRequest.Location);

                if (destinationArea == null)
                {
                    return NotFound(new { success = false, message = $"{messagePrefix} Destination: '{deliveryRequest.DestinationArea}' Not Found." });
                }

                if (currentLocation == null)
                {
                    return NotFound(new { success = false, message = $"{messagePrefix} Location: '{deliveryRequest.Location}' Not Found." });
                }

                (bool isValidCartLocation, string validationErrorMessage) = await DeliveryCommands.ValidateCurrentCartLocation(_svtContext, currentLocation, deliveryRequest.CartId);

                if (!isValidCartLocation)
                {
                    return Conflict(new { success = false, message = $"{messagePrefix} {validationErrorMessage}" });
                }

                var delivery = await DeliveryCommands.CreateNewDelivery(_svtContext, new DeliveryCommands.CreateNewDeliveryRequest
                {
                    CartId = deliveryRequest.CartId,
                    OrderId = deliveryRequest.OrderId,
                    DestinationAreaId = destinationArea.AreaId,
                    UserId = HttpContext.User.Identity.Name,
                    DeliveryType = deliveryRequest.DeliveryType
                });

                delivery.Queued = true;

                var lowestPriorityDelivery = await DeliveryCommands.GetLowestPriorityDelivery(_svtContext, currentLocation.Area.PoolId);

                if (lowestPriorityDelivery != null)
                {
                    delivery.PreviousPrioritizedDeliveryId = lowestPriorityDelivery.DeliveryId;
                }

                delivery.Locations = new List<Location> { currentLocation };

                await _svtContext.SaveChangesAsync();
            }

            await transaction.CommitAsync();

            return Ok(new { success = true, message = $"{messagePrefix}" });
        }

        /// <summary>
        /// Delivery to be created/queued
        /// </summary>
        public class DeliveryRequest
        {
            /// <summary>
            /// Gets/sets OrderId property
            /// </summary>
            /// <value>Order associated with delivery request</value>
            public string OrderId { get; set; }

            /// <summary>
            /// Gets/sets CartId property
            /// </summary>
            /// <value>Cart requested for delivery</value>
            public string CartId { get; set; }

            /// <summary>
            /// Gets/sets Location property
            /// </summary>
            /// <value>Requested starting location</value>
            public string Location { get; set; }

            /// <summary>
            /// Gets/sets DestinationArea property
            /// </summary>
            /// <value>Requested ultimate destination</value>
            public string DestinationArea { get; set; }

            /// <summary>
            /// Gets/sets DeliveryType property
            /// </summary>
            /// <value>Type of delivery</value>
            public string DeliveryType { get; set; }
        }

        /// <summary>
        /// Represents request body
        /// </summary>
        public class DeliveryRequests
        {
            /// <summary>
            /// Gets/sets Deliveries property
            /// </summary>
            /// <value>N number of DeliveryRequest instances to create/queue</value>
            public List<DeliveryRequest> Deliveries { get; set; }
        }
    }
}