using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SVT.Platform.Data.Models
{
    [Table("AreaDeliveryTypes")]
    public class AreaDeliveryType
    {
        [Required]
        public int AreaId { get; set; }

        [StringLength(50)]
        [Required]
        public string DeliveryType { get; set; }

        // NAVIGATION MEMBERS
        public virtual Area Area { get; set; }
        public virtual DeliveryType DeliveryTypeReference { get; set; }
    }
}