using System;
using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public abstract class BaseAuditModel
    {
        public DateTime InsertedOn { get; set; }

        [StringLength(256)]
        public string InsertedBy { get; set; }

        public DateTime ModifiedOn { get; set; }

        [StringLength(256)]
        public string ModifiedBy { get; set; }
    }
}