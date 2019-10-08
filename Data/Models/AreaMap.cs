using System.ComponentModel.DataAnnotations.Schema;

namespace SVT.Platform.Data.Models
{
    [Table("AreaMaps")]
    public class AreaMap
    {
        public int PreviousAreaId { get; set; }
        public int NextAreaId { get; set; }
        public bool Active { get; set; }

        // NAVIGATION MEMBERS
        public virtual Area PreviousArea { get; set; }
        public virtual Area NextArea { get; set; }
    }
}