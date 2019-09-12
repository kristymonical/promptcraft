using System;

namespace SVT.Platform.Data.Models
{
    public abstract class BaseModel<T>
    {
        public T Id { get; set; }
        public static Type IdType { get { return typeof(T); } }
        public DateTime Inserted { get; set; }
        public DateTime Updated { get; set; }
    }
}