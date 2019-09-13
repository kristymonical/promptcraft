using System.ComponentModel.DataAnnotations.Schema;

namespace SVT.Platform.Data.Models
{
    [Table("AreaMaps")]
    public class AreaMap : BaseModel<int>
    {
        public int SourceAreaId { get; set; }
        public int DestinationAreaId { get; set; }

        public Area SourceArea { get; set; }
        public Area DestinationArea { get; set; }
    }
}