using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class UserCommands
    {
        public static async Task<User> GetUserByName(SVTContext context, string name)
        {
            return await context.Users
                .Where(u => u.Name == name)
                .FirstOrDefaultAsync();
        }
    }
}