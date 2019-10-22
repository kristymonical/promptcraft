using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SVT.Platform.Data.Models
{
    public class Log : BaseAuditModel
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        public Guid TrackingId { get; set; } = new Guid();

        public int DeliveryId { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        internal string Serialized { get; set; }

        // NAVIGATION MEMBERS
        public virtual Delivery Delivery { get; set; }

        public virtual ActionType ActionTypeReference { get; set; }
    }
}