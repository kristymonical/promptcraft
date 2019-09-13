using System.ComponentModel.DataAnnotations;

namespace SVT.Platform.Data.Models
{
    public class Itinerary : BaseAuditModel
    {
        public int DeliveryId { get; set; }
        [StringLength(50)]
        [Required]
        public string ItineraryStatus { get; set; }
        [StringLength(50)]
        [Required]
        public string ItineraryType { get; set; }
        public int LocationId { get; set; }
        public int? PreviousItineraryId { get; set; }

        public Delivery Delivery { get; set; }
        public ItineraryStatus ItineraryStatusReference { get; set; }
        public ItineraryType ItineraryTypeReference { get; set; }
        public Location Location { get; set; }
        public Itinerary PreviousItinerary { get; set; }
        public Itinerary NextItinerary { get; set; }
    }
}