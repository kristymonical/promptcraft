using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class AreaType : BaseReferenceModel
    {
        // NAVIGATION MEMBERS
        public virtual ICollection<Area> Areas { get; set; }
    }
}