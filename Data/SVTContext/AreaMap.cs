using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public void BuildAreaMap(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AreaMap>(areaMapBuilder =>
            {
                areaMapBuilder
                    .HasKey(areaMap => new { areaMap.NextAreaId, areaMap.PreviousAreaId });
                areaMapBuilder
                    .HasOne(areaMap => areaMap.NextArea)
                    .WithMany(area => area.PreviousAreas)
                    .HasForeignKey(areaMap => areaMap.NextAreaId)
                    .OnDelete(DeleteBehavior.Restrict); // @database fix this stupid thing
                areaMapBuilder
                    .HasOne(areaMap => areaMap.PreviousArea)
                    .WithMany(area => area.NextAreas)
                    .HasForeignKey(areaMap => areaMap.PreviousAreaId)
                    .OnDelete(DeleteBehavior.Restrict); // @database fix this stupid thing
                areaMapBuilder
                    .HasData(
                        new AreaMap { PreviousAreaId = 1, NextAreaId = 3, Active = true },
                        new AreaMap { PreviousAreaId = 1, NextAreaId = 4, Active = true },
                        new AreaMap { PreviousAreaId = 1, NextAreaId = 5, Active = true },
                        new AreaMap { PreviousAreaId = 2, NextAreaId = 3, Active = true },
                        new AreaMap { PreviousAreaId = 2, NextAreaId = 4, Active = true },
                        new AreaMap { PreviousAreaId = 2, NextAreaId = 5, Active = true },
                        new AreaMap { PreviousAreaId = 3, NextAreaId = 4, Active = true },
                        new AreaMap { PreviousAreaId = 3, NextAreaId = 5, Active = true },
                        new AreaMap { PreviousAreaId = 4, NextAreaId = 3, Active = true },
                        new AreaMap { PreviousAreaId = 4, NextAreaId = 6, Active = true },
                        new AreaMap { PreviousAreaId = 5, NextAreaId = 3, Active = true },
                        new AreaMap { PreviousAreaId = 5, NextAreaId = 7, Active = true },
                        new AreaMap { PreviousAreaId = 6, NextAreaId = 8, Active = true },
                        new AreaMap { PreviousAreaId = 7, NextAreaId = 9, Active = true }
                    );
            });
        }
    }
}