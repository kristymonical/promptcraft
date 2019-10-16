using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class DeliveryType : BaseReferenceModel
    {
        // NAVIGATION MEMBERS
        public virtual ICollection<AreaDeliveryType> AreaDeliveryTypes { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
    }
}