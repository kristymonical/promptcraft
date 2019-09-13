using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SVT.Platform.Data.Models
{
    [Table("AreaDeliveryTypes")]
    public class AreaDeliveryType : BaseModel<int>
    {
        public int AreaId { get; set; }
        [StringLength(50)]
        public string DeliveryType { get; set; }

        public Area Area { get; set; }
        public DeliveryType DeliveryTypeReference { get; set; }
    }
}