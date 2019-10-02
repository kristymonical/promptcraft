using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Commands
{
    public partial class LocationCommands
    {
        public static async Task<Location> GetLocationByName(SVTContext context, string locationName)
        {
            return await context.Locations.Where(loc => loc.Name == locationName).FirstOrDefaultAsync();
        }
    }
}