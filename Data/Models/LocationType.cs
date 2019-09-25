using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class LocationType : BaseReferenceModel
    {
        // NAVIGATION MEMBERS
        public virtual ICollection<Location> Locations { get; set; }
    }
}