using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class Area : BaseModel<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<AreaPool> AreaPools { get; set; }
        public ICollection<DeliveryRequest> DeliveryRequests { get; set; }
        public ICollection<AreaToArea> FromAreas { get; set; }
        public ICollection<AreaToArea> ToAreas { get; set; }
        public ICollection<Position> Positions { get; set; }
    }
}
