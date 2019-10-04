using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DevLog> DevLogs { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }

        public SVTContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            BuildAreaType(modelBuilder);
            BuildPool(modelBuilder);
            BuildArea(modelBuilder);
            BuildLocationType(modelBuilder);
            BuildLocation(modelBuilder);
            BuildDeliveryType(modelBuilder);
            
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
                    .Property(d => d.InsertedBy)
                    .HasDefaultValueSql("suser_sname()");
                deliveryBuilder
                    .Property(d => d.InsertedOn)
                    .HasDefaultValueSql("getdate()");
                deliveryBuilder
                    .Property(d => d.ModifiedBy)
                    .HasDefaultValueSql("suser_sname()");
                deliveryBuilder
                    .Property(d => d.ModifiedOn)
                    .HasDefaultValueSql("getdate()");
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
                    .Property(d => d.InsertedBy)
                    .HasDefaultValueSql("suser_sname()");
                itineraryBuilder
                    .Property(d => d.InsertedOn)
                    .HasDefaultValueSql("getdate()");
                itineraryBuilder
                    .Property(d => d.ModifiedBy)
                    .HasDefaultValueSql("suser_sname()");
                itineraryBuilder
                    .Property(d => d.ModifiedOn)
                    .HasDefaultValueSql("getdate()");
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

            // Job FKs
            modelBuilder.Entity<Job>(jobBuilder =>
            {
                jobBuilder
                    .Property(d => d.InsertedBy)
                    .HasDefaultValueSql("suser_sname()");
                jobBuilder
                    .Property(d => d.InsertedOn)
                    .HasDefaultValueSql("getdate()");
                jobBuilder
                    .Property(d => d.ModifiedBy)
                    .HasDefaultValueSql("suser_sname()");
                jobBuilder
                    .Property(d => d.ModifiedOn)
                    .HasDefaultValueSql("getdate()");
                jobBuilder
                    .HasOne(job => job.Delivery)
                    .WithMany(delivery => delivery.Jobs)
                    .HasForeignKey(job => job.DeliveryId);
            });

            // ScheduledDelivery FKs
            modelBuilder.Entity<ScheduledDelivery>(scheduledDeliveryBuilder =>
            {
                scheduledDeliveryBuilder
                    .Property(d => d.InsertedBy)
                    .HasDefaultValueSql("suser_sname()");
                scheduledDeliveryBuilder
                    .Property(d => d.InsertedOn)
                    .HasDefaultValueSql("getdate()");
                scheduledDeliveryBuilder
                    .Property(d => d.ModifiedBy)
                    .HasDefaultValueSql("suser_sname()");
                scheduledDeliveryBuilder
                    .Property(d => d.ModifiedOn)
                    .HasDefaultValueSql("getdate()");
                scheduledDeliveryBuilder
                    .HasOne(scheduledDelivery => scheduledDelivery.Delivery)
                    .WithMany(delivery => delivery.ScheduledDeliveries)
                    .HasForeignKey(scheduledDelivery => scheduledDelivery.DeliveryId)
                    .OnDelete(DeleteBehavior.Restrict);
                scheduledDeliveryBuilder
                    .HasOne(scheduledDelivery => scheduledDelivery.DestinationLocation)
                    .WithMany(location => location.ScheduledDeliveries)
                    .HasForeignKey(scheduledDelivery => scheduledDelivery.DestinationLocationId)
                    .OnDelete(DeleteBehavior.Restrict);
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