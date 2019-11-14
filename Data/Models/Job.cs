using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Job : BaseAuditModel
    {
        [Key]
        [Required]
        public int JobId { get; set; }

        [Required]
        public int AethonJobId { get; set; }

        [Required]
        public int DeliveryId { get; set; }

        public DateTime? Completed { get; set; }

        public DateTime? Canceled { get; set; }

        public DateTime? Expired { get; set; }

        // NAVIGATION MEMBERS
        public virtual Delivery Delivery { get; set; }

        public virtual ICollection<Itinerary> Itineraries { get; set; }
    }
}