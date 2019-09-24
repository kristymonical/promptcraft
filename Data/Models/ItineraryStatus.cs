using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class ItineraryStatus : BaseReferenceModel
    {
        public ICollection<Itinerary> Itineraries { get; set; }
    }
}