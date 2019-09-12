namespace SVT.Platform.Data.Models
{
    public class AreaPool : BaseModel<int>
    {
        public int AreaId { get; set; }
        public int PoolId { get; set; }

        public Area Area { get; set; }
        public Pool Pool { get; set; }
    }
}