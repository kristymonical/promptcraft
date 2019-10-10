using System.Linq;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class JobCommands
    {
        public static IQueryable<Job> GetActiveJobs(SVTContext context)
        {
            return context.Jobs
                .Where(j => j.Completed == null)
                .Where(j => j.Canceled == null);
        }
    }
}