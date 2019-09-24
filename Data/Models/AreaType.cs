using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class AreaType : BaseReferenceModel
    {
        public ICollection<Area> Areas { get; set; }
    }
}