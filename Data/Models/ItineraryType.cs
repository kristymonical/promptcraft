using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class ItineraryType : BaseReferenceModel
    {
        public ICollection<Itinerary> Itineraries { get; set; }
    }
}