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

    public class CreateDeliveryRequestRequest
    {
        public string OrderId { get; set; }
        public string CartId { get; set; }
        public string Location { get; set; }
        public string DestinationArea { get; set; }
        public string DeliveryType { get; set; }
    }

    public class GetAreasResponse
    {
        public int AreaId { get; set; }
        public string AreaName { get; set; }
    }

    public class MoveCartRequest
    {
        public string MalLocationName { get; set; }
        public string CartId { get; set; }
    }

    public class GetOrderAndDestinationRequest
    {
        public string MalLocationName { get; set; }
        public string CartId { get; set; }
    }

    public class GetOrderAndDestinationResponse
    {
        public string OrderId { get; set; }
        public string DestinationAreaName { get; set; }
    }

    public class DeliveryResponse
    {
        public int DeliveryId { get; set; }
        public string UserId { get; set; }
        public string CartId { get; set; }
        public string OrderId { get; set; }
        public string CurrentLocation { get; set; }
        public string ReservedLocation { get; set; }
        public string DestinationArea { get; set; }
        public string DeliveryType { get; set; }
    }
}