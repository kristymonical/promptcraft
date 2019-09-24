using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class UserLog : BaseLogModel
    {
        [StringLength(256)]
        public string vUserId { get; set; }
    }
}