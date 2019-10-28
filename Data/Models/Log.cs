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
        public string TrackingId { get; set; }

        public int? DeliveryId { get; set; }

        [Required]
        public string Action { get; set; }

        [Required]
        public string Serialized { get; set; }

        // NAVIGATION MEMBERS
        public virtual Delivery Delivery { get; set; }

        public virtual ActionType ActionTypeReference { get; set; }
    }
}