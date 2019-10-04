using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<Area> Areas { get; set; }

        public void BuildArea(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(areaBuilder =>
            {
                areaBuilder
                    .HasOne(area => area.AreaTypeReference)
                    .WithMany(areaType => areaType.Areas)
                    .HasForeignKey(area => area.AreaType);
                areaBuilder
                    .HasOne(area => area.Pool)
                    .WithMany(pool => pool.Areas)
                    .HasForeignKey(area => area.PoolId);
                areaBuilder
                    .HasData(
                        new Area{ AreaId = 1, Name = "101B-FPA", AreaType = "fpa", PoolId = 1 },
                        new Area{ AreaId = 2, Name = "101C-CarWash", AreaType = "cw", PoolId = 1 },
                        new Area{ AreaId = 3, Name = "1430-Staging", AreaType = "stg", PoolId = 1 },
                        new Area{ AreaId = 4, Name = "1501-MAL-A", AreaType = "mal", PoolId = 1 } ,
                        new Area{ AreaId = 5, Name = "2501-MAL-A", AreaType = "mal", PoolId = 1 } ,
                        new Area{ AreaId = 6, Name = "1501-MAL-B", AreaType = "mal", PoolId = 2 } ,
                        new Area{ AreaId = 7, Name = "2501-MAL-B", AreaType = "mal", PoolId = 3 } ,
                        new Area{ AreaId = 8, Name = "15xx-Suite MALs", AreaType = "smal", PoolId = 2 } ,
                        new Area{ AreaId = 9, Name = "25xx-Suite MALs", AreaType = "smal", PoolId = 3 } 
                    );
            });
        }
    }
}