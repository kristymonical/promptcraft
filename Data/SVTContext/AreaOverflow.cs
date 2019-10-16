using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public void BuildAreaOverflow(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AreaOverflow>(areaOverflowBuilder =>
            {
                areaOverflowBuilder
                    .HasKey(areaOverflow => new { areaOverflow.DestinationAreaId, areaOverflow.OverflowAreaId });
                areaOverflowBuilder
                    .HasOne(areaOverflow => areaOverflow.DestinationArea)
                    .WithMany(area => area.AreaOverflows)
                    .HasForeignKey(areaOverflow => areaOverflow.DestinationAreaId)
                    .OnDelete(DeleteBehavior.Restrict);
                areaOverflowBuilder
                    .HasOne(areaOverflow => areaOverflow.OverflowArea)
                    .WithMany(area => area.AreasOverflowFor)
                    .HasForeignKey(areaOverflow => areaOverflow.OverflowAreaId)
                    .OnDelete(DeleteBehavior.Restrict);
                areaOverflowBuilder
                    .HasData(
                        new AreaOverflow { DestinationAreaId = 4, OverflowAreaId = 3, Priority = 1, Active = true },
                        new AreaOverflow { DestinationAreaId = 5, OverflowAreaId = 3, Priority = 1, Active = true },
                        new AreaOverflow { DestinationAreaId = 1, OverflowAreaId = 3, Priority = 1, Active = true },
                        new AreaOverflow { DestinationAreaId = 2, OverflowAreaId = 3, Priority = 1, Active = true }
                    );
            });
        }
    }
}