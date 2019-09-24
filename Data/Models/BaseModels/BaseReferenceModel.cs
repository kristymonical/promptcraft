using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public abstract class BaseReferenceModel<T>
    {
        [Key]
        [StringLength(50)]
        public T Value { get; set; }
        [StringLength(255)]
        public string Description { get; set; }
    }

    // overloaded class to default Value to string
    public abstract class BaseReferenceModel : BaseReferenceModel<string> { }
}