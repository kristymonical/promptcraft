using System.ComponentModel.DataAnnotations.Schema;

namespace SVT.Platform.Data.Models
{
    [Table("AreaMaps")]
    public class AreaMap : BaseModel
    {
        public int SourceAreaId { get; set; }
        public int DestinationAreaId { get; set; }

        // NAVIGATION MEMBERS
        public Area SourceArea { get; set; }
        public Area DestinationArea { get; set; }
    }
}