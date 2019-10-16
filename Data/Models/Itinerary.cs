using System;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Itinerary : BaseAuditModel
    {
        [Key]
        [Required]
        public int ItineraryId { get; set; }

        [Required]
        public int AethonRunId { get; set; }

        public DateTime? Completed { get; set; }

        public DateTime? TimedOut { get; set; }

        [Required]
        public int JobId { get; set; }

        [Required]
        public int LocationId { get; set; }

        // NAVIGATION MEMBERS
        public virtual Job Job { get; set; }
        public virtual Location Location { get; set; }
    }
}