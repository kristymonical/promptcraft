using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class UserCommands
    {
        public static async Task<User> GetUserByNameAsync(SVTContext context, string name)
        {
            return await context.Users
                .Where(u => u.Name == name)
                .FirstOrDefaultAsync();
        }

        public static User GetUserByName(SVTContext context, string name)
        {
            var userTask = GetUserByNameAsync(context, name);
            userTask.Wait();
            return userTask.Result;
        }
    }
}