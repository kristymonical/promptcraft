using System;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public abstract class BaseLogModel
    {
        [Required]
        public DateTime InsertedOn { get; set; }

        [StringLength(256)]
        [Required]
        public string InsertedBy { get; set; }

        [Required]
        public string Serialized { get; set; }
    }
}