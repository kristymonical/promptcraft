using System;

namespace SVT.Platform.Data.Models
{
    public class TripHistory : BaseModel<int>
    {
        public string Status { get; set; }
        public int TripRequestId { get; set; }

        public TripRequest TripRequest { get; set; }
    }
}