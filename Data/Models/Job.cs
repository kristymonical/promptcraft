using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Job : BaseAuditModel
    {
        [Key]
        public int JobId { get; set; }

        public int? AethonJobId { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        public int DeliveryId { get; set; }

        // NAVIGATION MEMBERS
        public Delivery Delivery { get; set; }
        public ICollection<Itinerary> Itineraries { get; set; }
    }
}