using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Area : BaseModel<int>
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        public int PoolId { get; set; }
        [StringLength(50)]
        [Required]
        public string AreaType { get; set; }

        public AreaType AreaTypeReference { get; set; }
        public Pool Pool { get; set; }
        public ICollection<AreaMap> SourceAreas { get; set; }
        public ICollection<AreaMap> DestinationAreas { get; set; }
        public ICollection<AreaDeliveryType> AreaDeliveryTypes { get; set; }
        public ICollection<Delivery> Deliveries { get; set; }
        public ICollection<Location> Locations { get; set; }
    }
}