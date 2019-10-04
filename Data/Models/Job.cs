using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Job : BaseAuditModel
    {
        [Key]
        public int JobId { get; set; }

        public int AethonJobId { get; set; }

        public int DeliveryId { get; set; }

        public DateTime Completed { get; set; }

        public DateTime Canceled { get; set; }

        public DateTime Expired { get; set; }

        // NAVIGATION MEMBERS
        public virtual Delivery Delivery { get; set; }
        
        public virtual ICollection<Itinerary> Itineraries { get; set; }
    }
}