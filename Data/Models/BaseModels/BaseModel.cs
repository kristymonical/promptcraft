using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public abstract class BaseModel<T>
    {
        [Required]
        public T Id { get; set; }
    }

    // overloaded class to default Id to int
    public abstract class BaseModel : BaseModel<int> { }
}