using System.ComponentModel.DataAnnotations;
using Microsoft.SqlServer.Types;

namespace SVT.Platform.Data.Models
{
    public abstract class BaseHierarchyModel
    {
        [Key]
        [Required]
        public SqlHierarchyId Node { get; set; }

        public short? NodeLevel { get; set; }
    }
}