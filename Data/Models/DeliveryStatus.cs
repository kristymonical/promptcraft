using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class DeliveryStatus : BaseReferenceModel
    {
        // NAVIGATION MEMBERS
        public ICollection<Delivery> Deliveries { get; set; }
    }
}