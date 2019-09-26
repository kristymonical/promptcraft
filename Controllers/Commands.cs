using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Types;
using SVT.Platform.Data;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Controllers
{
    public static class Commands
    {
        public static IQueryable<Location> GetStagedCartLocationsQuery(SVTContext context)
        {
            return context.Locations
                .Where(loc => loc.DeliveryId != null)
                .Where(loc => loc.Area.AreaType == "stg")
                .Where(loc => loc.Delivery.DeliveryType != "return");
        }

        public static async Task<Area> GetAreaByNameAsync(SVTContext context, string areaName)
        {
            return await context.Areas.Where(area => area.Name == areaName).FirstAsync();
        }

        public static async Task<Location> GetLocationByNameAsync(SVTContext context, string locationName)
        {
            return await context.Locations.Where(loc => loc.Name == locationName).FirstAsync();
        }

        public static async Task<Location> GetAvailableLocationInAreaAsync(SVTContext context, int areaId)
        {
            return await context.Locations.Where(loc => loc.AreaId == areaId && !loc.Reserved && string.IsNullOrWhiteSpace(loc.Delivery.CartId)).FirstAsync();
        }

        public static async Task<Location> GetChildLocationAsync(SVTContext context, SqlHierarchyId parentNode, int areaId)
        {
            var area = await context.Areas
                .Where(area => area.AreaId != areaId)
                .Where(area => area.AreaHierarchies.Any(ah => ah.Node.IsDescendantOf(parentNode).IsTrue))
                .Where(area => area.Locations.Any(loc => loc.DeliveryId == null))
                .FirstAsync();

            return area.Locations
                .Where(loc => loc.DeliveryId == null)
                .First();
        }

        // public static async Task MoveCart(SVTContext context, string cartId, int malLocationId)
        // {

        // }

        public static async Task<Location> GetAvailableAncestorLocation(SVTContext context, string destinationAreaName, string currentAreaName)
        {
            // var sp = $"exec usp_getAvailableAncestorLocation @currentAreaName=N'{currentAreaName}', @destinationAreaName=N'{destinationAreaName}'";
            // var result = await context.Database.ExecuteSqlRawAsync(sp);
            var query = context.Set<Location>().FromSqlRaw($@"
                with source as (
                    select 
                            c.AreaNode
                        from
                            dbo.Areas b with(nolock)
                            join dbo.AreaHierarchy c with(nolock) on b.Name = N'{currentAreaName}' and b.AreaId = c.AreaId
                        where
                            1=1
                            and c.AreaNodeLevel = 1
                ), destination as (
                    select
                        ah.AreaNode
                    from
                        dbo.Areas a with(nolock)
                        join dbo.AreaHierarchy ah with(nolock) on a.Name = N'{destinationAreaName}' and a.AreaId = ah.AreaId
                    where
                        1=1
                        and ah.AreaNodeLevel = 2
                        and ah.AreaNode.GetAncestor(1) = (select AreaNode from source)
                )
                select top 1
                    l.*
                from
                    dbo.AreaHierarchy ah with(nolock)
                    join dbo.Areas a with(nolock) on a.AreaId = ah.AreaId
                    join dbo.Locations l with(nolock) on a.AreaId = l.AreaId
                where
                    1=1
                    and ah.AreaNode <> (select AreaNode from destination)
                    and ah.AreaNode.IsDescendantOf((select AreaNode from destination)) = 1
                    and l.DeliveryId is null
                order by
                    ah.AreaNode;
                ");
            return await query.FirstAsync();
        }
    }
}