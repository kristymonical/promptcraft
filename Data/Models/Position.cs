using System;

namespace SVT.Platform.Data.Models
{
    public class Position : BaseModel<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime Reserved { get; set; }
        public DateTime Occupied { get; set; }
        public DateTime Cleared { get; set; }
        public int TripRequestId { get; set; }
        public int AreaId { get; set; }

        public Area Area { get; set; }
        public TripRequest TripRequest { get; set; }
    }
}