using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Delivery : BaseAuditModel
    {
        [StringLength(50)]
        [Required]
        public string CartId { get; set; }

        [StringLength(50)]
        public string OrderId { get; set; }

        public DateTime? Completed { get; set; }

        public DateTime? Cancelled { get; set; }

        [StringLength(256)]
        [Required]
        public string UserId { get; set; }

        [StringLength(50)]
        [Required]
        public string DeliveryType { get; set; }

        [Required]
        public int DestinationAreaId { get; set; }

        // NAVIGATION MEMBERS
        public DeliveryType DeliveryTypeReference { get; set; }
        public Area DestinationArea { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<Job> Jobs { get; set; }
    }
}