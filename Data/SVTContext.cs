using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public class SVTContext : DbContext
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<AreaType> AreaTypes { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryType> DeliveryTypes { get; set; }
        public DbSet<DevLog> DevLogs { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<Pool> Pools { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }

        public SVTContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // self referential area to area many-to-many
            modelBuilder.Entity<AreaMap>(areaMapBuilder =>
            {
                areaMapBuilder
                    .HasKey(areaMap => new { areaMap.DestinationAreaId, areaMap.SourceAreaId });
                areaMapBuilder
                    .HasOne(areaMap => areaMap.DestinationArea)
                    .WithMany(area => area.SourceAreas)
                    .HasForeignKey(areaMap => areaMap.DestinationAreaId)
                    .OnDelete(DeleteBehavior.Restrict); // @database fix this stupid thing
                areaMapBuilder
                    .HasOne(areaMap => areaMap.SourceArea)
                    .WithMany(area => area.DestinationAreas)
                    .HasForeignKey(areaMap => areaMap.SourceAreaId)
                    .OnDelete(DeleteBehavior.Restrict); // @database fix this stupid thing
            });

            // AreaDeliveryTypes many-to-many
            modelBuilder.Entity<AreaDeliveryType>(areaDeliveryTypeBuilder =>
            {
                areaDeliveryTypeBuilder
                    .HasKey(areaDeliveryType => new { areaDeliveryType.AreaId, areaDeliveryType.DeliveryType });
                areaDeliveryTypeBuilder
                    .HasOne(areaDeliveryType => areaDeliveryType.Area)
                    .WithMany(area => area.AreaDeliveryTypes)
                    .HasForeignKey(areaDeliveryType => areaDeliveryType.AreaId)
                    .OnDelete(DeleteBehavior.Restrict); // @database fix this stupid thing
                areaDeliveryTypeBuilder
                    .HasOne(areaDeliveryType => areaDeliveryType.DeliveryTypeReference)
                    .WithMany(deliveryType => deliveryType.AreaDeliveryTypes)
                    .HasForeignKey(areaDeliveryType => areaDeliveryType.DeliveryType)
                    .OnDelete(DeleteBehavior.Restrict); // @database fix this stupid thing
            });

            // Area FKs
            modelBuilder.Entity<Area>(areaBuilder =>
            {
                areaBuilder
                    .HasOne(area => area.AreaTypeReference)
                    .WithMany(areaType => areaType.Areas)
                    .HasForeignKey(area => area.AreaType);
                areaBuilder
                    .HasOne(area => area.Pool)
                    .WithMany(pool => pool.Areas)
                    .HasForeignKey(area => area.PoolId);
            });

            // AreaHierarchy FKs
            modelBuilder.Entity<AreaHierarchy>(areaHierarchyBuilder =>
            {
                areaHierarchyBuilder
                    .HasOne(areaHierarchy => areaHierarchy.Area)
                    .WithMany(area => area.AreaHierarchies)
                    .HasForeignKey(areaHierarchy => areaHierarchy.AreaId);
            });

            // Delivery FKs
            modelBuilder.Entity<Delivery>(deliveryBuilder =>
            {
                deliveryBuilder
                    .HasOne(delivery => delivery.DeliveryTypeReference)
                    .WithMany(deliveryType => deliveryType.Deliveries)
                    .HasForeignKey(delivery => delivery.DeliveryType);
                deliveryBuilder
                    .HasOne(delivery => delivery.DestinationArea)
                    .WithMany(area => area.Deliveries)
                    .HasForeignKey(delivery => delivery.DestinationAreaId);
                deliveryBuilder
                    .HasOne(delivery => delivery.PreviousPrioritizedDelivery)
                    .WithOne(delivery => delivery.NextPrioritizedDelivery)
                    .HasForeignKey<Delivery>(delivery => delivery.PreviousPrioritizedDeliveryId);
            });

            // Itinerary FKs
            modelBuilder.Entity<Itinerary>(itineraryBuilder =>
            {
                itineraryBuilder
                    .HasOne(itinerary => itinerary.Location)
                    .WithMany(location => location.Itineraries)
                    .HasForeignKey(itinerary => itinerary.LocationId)
                    .OnDelete(DeleteBehavior.Restrict); // @database fix this stupid thing
                itineraryBuilder
                    .HasOne(itinerary => itinerary.Job)
                    .WithMany(job => job.Itineraries)
                    .HasForeignKey(itinerary => itinerary.JobId);
            });

            // Location FKs
            modelBuilder.Entity<Location>(locationBuilder =>
            {
                locationBuilder
                    .HasOne(location => location.LocationTypeReference)
                    .WithMany(locationType => locationType.Locations)
                    .HasForeignKey(location => location.LocationType);
                locationBuilder
                    .HasOne(location => location.Area)
                    .WithMany(area => area.Locations)
                    .HasForeignKey(location => location.AreaId);
                locationBuilder
                    .HasOne(location => location.Delivery)
                    .WithMany(delivery => delivery.Locations)
                    .HasForeignKey(location => location.DeliveryId)
                    .IsRequired(false);
            });

            // Job FKs
            modelBuilder.Entity<Job>(jobBuilder =>
            {
                jobBuilder
                    .HasOne(job => job.Delivery)
                    .WithMany(delivery => delivery.Jobs)
                    .HasForeignKey(job => job.DeliveryId);
            });

            // Computed Fields
            modelBuilder.Entity<UserLog>()
                .Property(log => log.vUserId)
                .HasComputedColumnSql("CONVERT([nvarchar](256),json_value([Serialized],N'$.UserId'))");

            modelBuilder.Entity<AreaHierarchy>()
                .Property(hierarchy => hierarchy.NodeLevel)
                .HasComputedColumnSql("[Node].[GetLevel]()");
        }
    }
}