using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<ActionType> ActionTypes { get; set; }

        public void BuildActionType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActionType>(ActionTypeBuilder =>
            {
                ActionTypeBuilder
                    .HasData(
                        new ActionType { Value = "unknown" },
                        new ActionType { Value = "queue" },
                        new ActionType { Value = "schedule" },
                        new ActionType { Value = "status" },
                        new ActionType { Value = "move" },
                        new ActionType { Value = "clean" }
                    );
            });
        }
    }
}