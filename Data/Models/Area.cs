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

        public List<Area> GetAreasBy(GraphDirection direction, Func<Area, Area, bool> predicate)
        {
            var ancestors = new List<Area>();
            var nodes = new Stack<Area>();
            var visited = new HashSet<Area>();

            nodes.Push(this);

            while (nodes.Count > 0)
            {
                var node = nodes.Pop();

                if (visited.Contains(node)) continue;

                visited.Add(node);

                List<Area> nextNodes;

                if (direction == GraphDirection.ancestors) nextNodes = node.GetAncestors();
                else nextNodes = node.GetDescendants();

                foreach (var child in nextNodes)
                    if (!visited.Contains(child)) nodes.Push(child);

                if (predicate(this, node)) ancestors.Add(node);
            }

            return ancestors;
        }

        public enum GraphDirection { none, ancestors, descendants };

        public List<Area> GetAdjacentAreas()
        {
            var adjacent = this.GetAncestors();
            adjacent.AddRange(GetDescendants());

            return adjacent;
        }

        public List<Area> GetAncestors()
        {
            return this.PreviousAreas
                .Select(a => a.PreviousArea)
                .ToList();
        }

        public List<Area> GetDescendants()
        {
            return this.NextAreas
                .Select(a => a.NextArea)
                .ToList();
        }

        public List<Area> GetOverflowAreas()
        {
            return this.AreaOverflows
                .Select(o => o.OverflowArea)
                .ToList();
        }

        public bool IsAdjacentTo(GraphDirection direction, Area area)
        {
            if (direction == GraphDirection.ancestors) return this.IsDescendantOf(area);

            return this.IsAncestorOf(area);
        }

        public bool IsAncestorOf(Area area)
        {
            return this.GetDescendants().Contains(area);
        }

        public bool IsDescendantOf(Area area)
        {
            return this.GetAncestors().Contains(area);
        }

        public bool IsOverflowFor(Area primary)
        {
            return primary != null && primary.GetOverflowAreas().Contains(this);
        }
    }
}