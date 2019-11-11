using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<Pool> Pools { get; set; }

        public void BuildPool(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pool>(poolBuilder =>
            {
                poolBuilder
                    .HasData(
                        new Pool { PoolId = 1, Name = "Pool 1", Threshold = 2 },
                        new Pool { PoolId = 2, Name = "Pool 2", Threshold = 1 },
                        new Pool { PoolId = 3, Name = "Pool 3", Threshold = 1 }
                    );
            });
        }
    }
}