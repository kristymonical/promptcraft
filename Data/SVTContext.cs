using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public class SVTContext : DbContext
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<AreaType> AreaTypes { get; set; }
        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryStatus> DeliveryStatuses { get; set; }
        public DbSet<DeliveryType> DeliveryTypes { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<ItineraryStatus> ItineraryStatuses { get; set; }
        public DbSet<ItineraryType> ItineraryTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationType> LocationTypes { get; set; }
        public DbSet<Pool> Pools { get; set; }

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
                    .OnDelete(DeleteBehavior.ClientSetNull);
                areaMapBuilder
                    .HasOne(areaMap => areaMap.SourceArea)
                    .WithMany(area => area.DestinationAreas)
                    .HasForeignKey(areaMap => areaMap.SourceAreaId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Area / DeliveryType many-to-many
            modelBuilder.Entity<AreaDeliveryType>(areaDeliveryTypeBuilder =>
            {
                areaDeliveryTypeBuilder
                    .HasKey(areaDeliveryType => new { areaDeliveryType.AreaId, areaDeliveryType.DeliveryType });
                areaDeliveryTypeBuilder
                    .HasOne(areaDeliveryType => areaDeliveryType.DeliveryTypeReference)
                    .WithMany(deliveryType => deliveryType.AreaDeliveryTypes)
                    .HasForeignKey(areaDeliveryType => areaDeliveryType.DeliveryType);
                areaDeliveryTypeBuilder
                    .HasOne(areaDeliveryType => areaDeliveryType.Area)
                    .WithMany(area => area.AreaDeliveryTypes)
                    .HasForeignKey(areaDeliveryType => areaDeliveryType.AreaId);
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

            // Delivery FKs
            modelBuilder.Entity<Delivery>(deliveryBuilder =>
            {
                deliveryBuilder
                    .HasOne(delivery => delivery.DeliveryStatusReference)
                    .WithMany(deliveryStatus => deliveryStatus.Deliveries)
                    .HasForeignKey(delivery => delivery.DeliveryStatus);
                deliveryBuilder
                    .HasOne(delivery => delivery.DeliveryTypeReference)
                    .WithMany(deliveryType => deliveryType.Deliveries)
                    .HasForeignKey(delivery => delivery.DeliveryType);
                deliveryBuilder
                    .HasOne(delivery => delivery.DestinationArea)
                    .WithMany(area => area.Deliveries)
                    .HasForeignKey(delivery => delivery.DestinationAreaId);
            });

            // Itinerary FKs
            modelBuilder.Entity<Itinerary>(itineraryBuilder =>
            {
                itineraryBuilder
                    .HasOne(itinerary => itinerary.Delivery)
                    .WithMany(delivery => delivery.Itineraries)
                    .HasForeignKey(itinerary => itinerary.DeliveryId);
                itineraryBuilder
                    .HasOne(itinerary => itinerary.ItineraryStatusReference)
                    .WithMany(itineraryStatus => itineraryStatus.Itineraries)
                    .HasForeignKey(itinerary => itinerary.ItineraryStatus);
                itineraryBuilder
                    .HasOne(itinerary => itinerary.ItineraryTypeReference)
                    .WithMany(itineraryType => itineraryType.Itineraries)
                    .HasForeignKey(itinerary => itinerary.ItineraryType);
                itineraryBuilder
                    .HasOne(itinerary => itinerary.Location)
                    .WithMany(location => location.Itineraries)
                    .HasForeignKey(itinerary => itinerary.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                itineraryBuilder
                    .HasOne(itinerary => itinerary.PreviousItinerary)
                    .WithOne(previousItinerary => previousItinerary.NextItinerary)
                    .HasForeignKey<Itinerary>(itinerary => itinerary.PreviousItineraryId)
                    .IsRequired(false);
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
            });
        }
    }
}