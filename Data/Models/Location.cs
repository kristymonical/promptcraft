using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Location : BaseAuditModel
    {
        [Key]
        public int LocationId { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public int? DeliveryId { get; set; }

        public bool Reserved { get; set; }

        [StringLength(50)]
        [Required]
        public string LocationType { get; set; }

        public int AreaId { get; set; }

        /* NAVIGATION MEMBERS */
        public virtual LocationType LocationTypeReference { get; set; }
        public virtual Area Area { get; set; }
        public virtual Delivery Delivery { get; set; }
        public virtual ICollection<Itinerary> Itineraries { get; set; }
        public virtual ICollection<ScheduledDelivery> ScheduledDeliveries { get; set; }
    }
}