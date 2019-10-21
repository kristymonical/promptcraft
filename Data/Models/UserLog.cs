using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class UserLog : BaseLogModel
    {
        [Key]
        public int UserLogId { get; set; }
    }
}