using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Area : BaseModel
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        public int PoolId { get; set; }

        [StringLength(50)]
        [Required]
        public string AreaType { get; set; }

        // NAVIGATION MEMBERS
        public AreaType AreaTypeReference { get; set; }
        public Pool Pool { get; set; }
        public ICollection<AreaMap> SourceAreas { get; set; }
        public ICollection<AreaMap> DestinationAreas { get; set; }
        public ICollection<AreaDeliveryType> AreaDeliveryTypes { get; set; }
        public ICollection<Delivery> Deliveries { get; set; }
        public ICollection<Location> Locations { get; set; }
        public AreaHierarchy AreaHierarchyReference { get; set; }
    }
}