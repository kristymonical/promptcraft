using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class TripRequest : BaseModel<int>
    {
        public string CartId { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public int? PreviousTripRequestId { get; set; }
        public int DeliveryRequestId { get; set; }
        public int PositionId { get; set; }

        public Position Position { get; set; }
        public TripRequest PreviousTripRequest { get; set; }
        public DeliveryRequest DeliveryRequest { get; set; }
        public ICollection<TripHistory> TripHistories { get; set; }
    }
}