using System;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public abstract class BaseAuditModel<T> : BaseModel<T>
    {
        public DateTime InsertedOn { get; set; }
        [StringLength(256)]
        [Required]
        public string InsertedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        [StringLength(256)]
        [Required]
        public string ModifiedBy { get; set; }
    }

    public abstract class BaseAuditModel : BaseAuditModel<int> { }
}