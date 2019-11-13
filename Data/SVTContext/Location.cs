using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }

        public void BuildLocation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>(locationBuilder =>
            {
                locationBuilder
                    .Property(d => d.InsertedBy)
                    .HasDefaultValueSql("suser_sname()");
                locationBuilder
                    .Property(d => d.InsertedOn)
                    .HasDefaultValueSql("getutcdate()");
                locationBuilder
                    .Property(d => d.ModifiedBy)
                    .HasDefaultValueSql("suser_sname()");
                locationBuilder
                    .Property(d => d.ModifiedOn)
                    .HasDefaultValueSql("getutcdate()");
                locationBuilder
                    .Property(d => d.Reserved)
                    .HasDefaultValue(0);
                locationBuilder
                    .HasOne(location => location.LocationTypeReference)
                    .WithMany(locationType => locationType.Locations)
                    .HasForeignKey(location => location.LocationType);
                locationBuilder
                    .HasOne(location => location.Area)
                    .WithMany(area => area.Locations)
                    .HasForeignKey(location => location.AreaId);
                locationBuilder
                    .HasOne(location => location.Delivery)
                    .WithMany(delivery => delivery.Locations)
                    .HasForeignKey(location => location.DeliveryId)
                    .IsRequired(false);
                locationBuilder
                    .HasData(
                        new Location { LocationId = 1, Name = "101B-FPA-001", LocationType = "fpa", AreaId = 1 },
                        new Location { LocationId = 2, Name = "101B-FPA-002", LocationType = "fpa", AreaId = 1 },
                        new Location { LocationId = 3, Name = "101B-FPA-003", LocationType = "fpa", AreaId = 1 },
                        new Location { LocationId = 4, Name = "101B-FPA-004", LocationType = "fpa", AreaId = 1 },
                        new Location { LocationId = 5, Name = "101B-FPA-005", LocationType = "fpa", AreaId = 1 },
                        new Location { LocationId = 6, Name = "101C-CW-001", LocationType = "cw", AreaId = 2 },
                        new Location { LocationId = 7, Name = "101C-CW-002", LocationType = "cw", AreaId = 2 },
                        new Location { LocationId = 8, Name = "101C-CW-003", LocationType = "cw", AreaId = 2 },
                        new Location { LocationId = 9, Name = "101C-CW-004", LocationType = "cw", AreaId = 2 },
                        new Location { LocationId = 10, Name = "101C-CW-005", LocationType = "cw", AreaId = 2 },
                        new Location { LocationId = 11, Name = "1430-STG-001", LocationType = "stg", AreaId = 3 },
                        new Location { LocationId = 12, Name = "1430-STG-002", LocationType = "stg", AreaId = 3 },
                        new Location { LocationId = 13, Name = "1430-STG-003", LocationType = "stg", AreaId = 3 },
                        new Location { LocationId = 14, Name = "1430-STG-004", LocationType = "stg", AreaId = 3 },
                        new Location { LocationId = 15, Name = "1430-STG-005", LocationType = "stg", AreaId = 3 },
                        new Location { LocationId = 16, Name = "1430-STG-006", LocationType = "stg", AreaId = 3 },
                        new Location { LocationId = 17, Name = "1430-STG-007", LocationType = "stg", AreaId = 3 },
                        new Location { LocationId = 18, Name = "1430-STG-008", LocationType = "stg", AreaId = 3 },
                        new Location { LocationId = 19, Name = "1430-STG-009", LocationType = "stg", AreaId = 3 },
                        new Location { LocationId = 20, Name = "1430-STG-010", LocationType = "stg", AreaId = 3 },
                        new Location { LocationId = 21, Name = "1501-MAL-A-001", LocationType = "mal", AreaId = 4 },
                        new Location { LocationId = 22, Name = "1501-MAL-A-WAIT-001", LocationType = "wait", AreaId = 4 },
                        new Location { LocationId = 23, Name = "1501-MAL-A-WAIT-002", LocationType = "wait", AreaId = 4 },
                        new Location { LocationId = 24, Name = "1501-MAL-A-WAIT-003", LocationType = "wait", AreaId = 4 },
                        new Location { LocationId = 25, Name = "2501-MAL-A-001", LocationType = "mal", AreaId = 5 },
                        new Location { LocationId = 26, Name = "2501-MAL-A-WAIT-001", LocationType = "wait", AreaId = 5 },
                        new Location { LocationId = 27, Name = "2501-MAL-A-WAIT-002", LocationType = "wait", AreaId = 5 },
                        new Location { LocationId = 28, Name = "2501-MAL-A-WAIT-003", LocationType = "wait", AreaId = 5 },
                        new Location { LocationId = 29, Name = "1501-MAL-B-001", LocationType = "mal", AreaId = 6 },
                        new Location { LocationId = 30, Name = "1501-MAL-B-WAIT-001", LocationType = "wait", AreaId = 6 },
                        new Location { LocationId = 31, Name = "1501-MAL-B-WAIT-002", LocationType = "wait", AreaId = 6 },
                        new Location { LocationId = 32, Name = "1501-MAL-B-WAIT-003", LocationType = "wait", AreaId = 6 },
                        new Location { LocationId = 33, Name = "2501-MAL-B-001", LocationType = "mal", AreaId = 7 },
                        new Location { LocationId = 34, Name = "2501-MAL-B-WAIT-001", LocationType = "wait", AreaId = 7 },
                        new Location { LocationId = 35, Name = "2501-MAL-B-WAIT-002", LocationType = "wait", AreaId = 7 },
                        new Location { LocationId = 36, Name = "2501-MAL-B-WAIT-003", LocationType = "wait", AreaId = 7 },
                        new Location { LocationId = 37, Name = "1528-MAL-001", LocationType = "smal", AreaId = 8 },
                        new Location { LocationId = 38, Name = "1528-MAL-WAIT-001", LocationType = "wait", AreaId = 8 },
                        new Location { LocationId = 39, Name = "1528-MAL-WAIT-002", LocationType = "wait", AreaId = 8 },
                        new Location { LocationId = 40, Name = "1528-MAL-WAIT-003", LocationType = "wait", AreaId = 8 },
                        new Location { LocationId = 41, Name = "2519-MAL-001", LocationType = "smal", AreaId = 9 },
                        new Location { LocationId = 42, Name = "2519-MAL-WAIT-001", LocationType = "wait", AreaId = 9 },
                        new Location { LocationId = 43, Name = "2519-MAL-WAIT-002", LocationType = "wait", AreaId = 9 },
                        new Location { LocationId = 44, Name = "2519-MAL-WAIT-003", LocationType = "wait", AreaId = 9 }
                    );
            });
        }
    }
}