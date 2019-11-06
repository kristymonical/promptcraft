using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;

namespace SVT.Platform.Commands
{
    public partial class ContextCommands
    {
        public static void RollbackChanges(SVTContext context)
        {
            foreach (var entry in context.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }
    }
}