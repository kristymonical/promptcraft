using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        public virtual ICollection<AreaOverflow> AreaOverflows { get; set; }
        public virtual ICollection<AreaOverflow> AreasOverflowFor { get; set; }
        public virtual ICollection<AreaDeliveryType> AreaDeliveryTypes { get; set; }
        public virtual ICollection<Delivery> Deliveries { get; set; }
        public virtual ICollection<Location> Locations { get; set; }

        public static List<Area> GetLeafNodes(Area startingNode)
        {
            var accumulator = new List<Area>();

            bool _getLeafNodes(Area currentNode)
            {
                if (currentNode.AreaId != startingNode.AreaId && currentNode.NextAreas.Count == 0)
                {
                    accumulator.Add(currentNode);
                }

                foreach (var areaMap in currentNode.NextAreas)
                {
                    _getLeafNodes(areaMap.NextArea);
                }

                return true;
            }

            _getLeafNodes(startingNode);

            return accumulator;
        }

        public bool IsOverflowFor(Area primary)
        {
            if (primary == null) return false;

            return primary.AreaOverflows
                .Select(o => o.OverflowAreaId)
                .Contains(this.AreaId);
        }
    }
}