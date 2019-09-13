using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class LocationType : BaseReferenceModel
    {
        public ICollection<Location> Locations { get; set; }
    }
}