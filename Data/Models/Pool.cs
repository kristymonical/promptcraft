using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Pool : BaseModel<int>
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        public ICollection<Area> Areas { get; set; }
    }
}