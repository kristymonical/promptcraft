using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class ActionCommands
    {
        public static async Task<ActionType> GetActionByValueAsync(SVTContext context, string value)
        {
            return await context.ActionTypes
                .Where(a => a.Value == value)
                .FirstOrDefaultAsync();
        }

        public static ActionType GetActionByValue(SVTContext context, string value)
        {
            var actionTask = GetActionByValueAsync(context, value);
            actionTask.Wait();
            return actionTask.Result;
        }
    }
}