using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Location : BaseAuditModel
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        public int? CartId { get; set; }
        public bool Reserved { get; set; }
        [StringLength(50)]
        [Required]
        public string LocationType { get; set; }
        public int AreaId { get; set; }

        public LocationType LocationTypeReference { get; set; }
        public Area Area { get; set; }
        public ICollection<Itinerary> Itineraries { get; set; }
    }
}