using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Itinerary : BaseAuditModel
    {
        [Key]
        public int ItineraryId { get; set; }

        public int? AethonRunId { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        public int JobId { get; set; }

        public int LocationId { get; set; }

        // NAVIGATION MEMBERS
        public virtual Job Job { get; set; }
        public virtual Location Location { get; set; }
    }
}