using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Itinerary : BaseAuditModel
    {
        public int? AethonRunId { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        public int JobId { get; set; }

        public int LocationId { get; set; }

        // NAVIGATION MEMBERS
        public Job Job { get; set; }
        public Location Location { get; set; }
    }
}