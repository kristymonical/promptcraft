namespace SVT.Platform.Data.Models
{
    public class AreaHierarchy : BaseHierarchyModel
    {
        public int AreaId { get; set; }

        // NAVIGATION MEMBERS
        public Area Area { get; set; }
    }
}