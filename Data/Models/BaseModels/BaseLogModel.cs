using System;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public abstract class BaseLogModel<T> : BaseModel<T>
    {
        [Required]
        public DateTime InsertedOn { get; set; }

        [StringLength(256)]
        [Required]
        public string InsertedBy { get; set; }

        [Required]
        public string Serialized { get; set; }
    }

    // overloaded class to default Id to int
    public abstract class BaseLogModel : BaseLogModel<int> { }
}