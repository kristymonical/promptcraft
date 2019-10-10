using System.ComponentModel.DataAnnotations.Schema;

namespace SVT.Platform.Data.Models
{
    [Table("AreaOverflow")]
    public class AreaOverflow
    {
        public int DestinationAreaId { get; set; }
        public int OverflowAreaId { get; set; }
        public int Priority { get; set; }
        public bool Active { get; set; }

        // NAVIGATION MEMBERS

        public virtual Area DestinationArea { get; set; }
        public virtual Area OverflowArea { get; set; }
    }
}
