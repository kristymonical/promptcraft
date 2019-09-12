namespace SVT.Platform.Data.Models
{
    public class AreaToArea : BaseModel<int>
    {
        public int FromAreaId { get; set; }
        public int ToAreaId { get; set; }

        public Area FromArea { get; set; }
        public Area ToArea { get; set; }
    }
}