using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Pool
    {
        [Key]
        public int PoolId { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        public int Threshold { get; set; }

        // NAVIGATION MEMBERS
        public virtual ICollection<Area> Areas { get; set; }
    }
}