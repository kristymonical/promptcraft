namespace SVT.Platform.Controllers
{
    public class GetStagedCartsResponse
    {
        public string OrderId { get; set; }
        public string CartId { get; set; }
        public int StagingLocationId { get; set; }
        public string DeliveryRequestType { get; set; }
        public int DestinationArea { get; set; }
    }
}