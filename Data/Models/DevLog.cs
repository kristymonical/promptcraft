using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class DevLog : BaseLogModel
    {
        [Key]
        public int DevLogId { get; set; }
    }
}