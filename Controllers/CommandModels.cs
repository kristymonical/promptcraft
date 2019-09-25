namespace SVT.Platform.Controllers
{
    public class GetStagedCartsResponse
    {
        public string OrderId { get; set; }
        public string CartId { get; set; }
        public string StagingLocationId { get; set; }
        public string DeliveryRequestType { get; set; }
        public string DestinationArea { get; set; }
    }
}