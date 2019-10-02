using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class AreaCommands
    {
        public static async Task<Area> GetAreaByName(SVTContext context, string areaName)
        {
            return await context.Areas
                .Where(area => area.Name == areaName)
                .FirstOrDefaultAsync();
        }
    }
}