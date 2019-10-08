using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Area
    {
        [Key]
        public int AreaId { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        public int PoolId { get; set; }

        [StringLength(50)]
        [Required]
        public string AreaType { get; set; }

        // NAVIGATION MEMBERS
        public virtual AreaType AreaTypeReference { get; set; }
        public virtual Pool Pool { get; set; }
        public virtual ICollection<AreaMap> PreviousAreas { get; set; }
        public virtual ICollection<AreaMap> NextAreas { get; set; }
        public virtual ICollection<AreaDeliveryType> AreaDeliveryTypes { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<Location> Locations { get; set; }

        public bool GetLeafNodes(List<Area> accumulator)
        {
            if (this.NextAreas.Count == 0)
            {
                accumulator.Add(this);
            }

            foreach (var areaMap in this.NextAreas)
            {
                areaMap.NextArea.GetLeafNodes(accumulator);
            }

            return true;
        }
    }
}