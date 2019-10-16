using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class ScheduledDelivery : BaseAuditModel
    {
        [Key]
        public int ScheduledDeliveryId { get; set; }

        public DateTime TTL { get; set; }

        public DateTime Completed { get; set; }

        public DateTime Canceled { get; set; }

        public int DeliveryId { get; set; }

        public int DestinationLocationId { get; set; }

        // NAVIGATION MEMBERS
        public virtual Delivery Delivery { get; set; }
        
        public virtual Location DestinationLocation { get; set; }
    }
}