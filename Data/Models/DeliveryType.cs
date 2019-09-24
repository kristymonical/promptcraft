using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class DeliveryType : BaseReferenceModel
    {
        // NAVIGATION MEMBERS
        public ICollection<AreaDeliveryType> AreaDeliveryTypes { get; set; }
        public ICollection<Delivery> Deliveries { get; set; }
    }
}