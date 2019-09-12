namespace SVT.Platform.Data.Models
{
    public class Job : BaseModel<int>
    {
        public string Status { get; set; }
        public string DetailsSerialized { get; set; }
        public int TripRequestId { get; set; }

        public TripRequest TripRequest { get; set; }
    }
}