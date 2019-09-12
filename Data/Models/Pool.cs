using System.Collections.Generic;

namespace SVT.Platform.Data.Models
{
    public class Pool : BaseModel<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<AreaPool> AreaPools { get; set; }
    }
}