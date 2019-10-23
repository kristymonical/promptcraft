using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public void BuildUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(userBuilder =>
            {
                userBuilder
                    .HasData(
                        new User { UserId = 1, Name = "API" },
                        new User { UserId = 2, Name = "DevAPI" },
                        new User { UserId = 3, Name = "ScheduleService" },
                        new User { UserId = 4, Name = "StatusService" }
                    );
            });
        }
    }
}