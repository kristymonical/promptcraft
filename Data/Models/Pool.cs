using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Pool : BaseModel
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        // NAVIGATION MEMBERS
        public ICollection<Area> Areas { get; set; }
    }
}