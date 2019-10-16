using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<AreaType> AreaTypes { get; set; }

        public void BuildAreaType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AreaType>(areaTypeBuilder =>
            {
                areaTypeBuilder
                    .HasData(
                        new AreaType{ Value = "cw" },
                        new AreaType{ Value = "fpa" },
                        new AreaType{ Value = "mal" },
                        new AreaType{ Value = "smal" },
                        new AreaType{ Value = "stg" }
                    );
            });
        }
    }
}