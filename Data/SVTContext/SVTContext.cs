using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public SVTContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildAreaType(modelBuilder);
            BuildPool(modelBuilder);
            BuildArea(modelBuilder);
            BuildAreaMap(modelBuilder);
            BuildAreaOverflow(modelBuilder);
            BuildAreaDeliveryType(modelBuilder);
            BuildLocationType(modelBuilder);
            BuildLocation(modelBuilder);
            BuildActionType(modelBuilder);
            BuildLog(modelBuilder);
            BuildDeliveryType(modelBuilder);
            BuildDelivery(modelBuilder);
            BuildItinerary(modelBuilder);
            BuildJob(modelBuilder);
            BuildUser(modelBuilder);
        }
    }
}