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

        // NAVIGATION MEMBERS
        public ICollection<Area> Areas { get; set; }
    }
}