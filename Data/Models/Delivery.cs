using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Delivery : BaseAuditModel
    {
        public int? OrderId { get; set; }
        public DateTime? Completed { get; set; }
        [StringLength(256)]
        [Required]
        public string UserId { get; set; }
        [StringLength(50)]
        [Required]
        public string DeliveryStatus { get; set; }
        [StringLength(50)]
        [Required]
        public string DeliveryType { get; set; }
        public int DestinationAreaId { get; set; }

        public DeliveryStatus DeliveryStatusReference { get; set; }
        public DeliveryType DeliveryTypeReference { get; set; }
        public Area DestinationArea { get; set; }
        public ICollection<Itinerary> Itineraries { get; set; }
    }
}