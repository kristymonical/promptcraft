using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class DeliveryRequest : BaseModel<int>
    {
        public string LonzaOrderId { get; set; }
        public bool Completed { get; set; }
        public int AreaId { get; set; }

        public Area Area { get; set; }
        public ICollection<TripRequest> TripRequests { get; set; }
    }
}