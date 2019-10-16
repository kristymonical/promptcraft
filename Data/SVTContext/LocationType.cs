using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<LocationType> LocationTypes { get; set; }

        public void BuildLocationType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocationType>(locationTypeBuilder =>
            {
                locationTypeBuilder
                    .HasData(
                        new LocationType{ Value = "cw" },
                        new LocationType{ Value = "fpa" },
                        new LocationType{ Value = "mal" },
                        new LocationType{ Value = "smal" },
                        new LocationType{ Value = "stg" },
                        new LocationType{ Value = "wait" }
                    );
            });
        }
    }
}