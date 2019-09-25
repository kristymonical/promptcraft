using System.ComponentModel.DataAnnotations.Schema;

namespace SVT.Platform.Data.Models
{
    [Table("AreaMaps")]
    public class AreaMap
    {
        public int SourceAreaId { get; set; }
        public int DestinationAreaId { get; set; }

        // NAVIGATION MEMBERS
        public virtual Area SourceArea { get; set; }
        public virtual Area DestinationArea { get; set; }
    }
}