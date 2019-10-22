using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<Delivery> Deliveries { get; set; }

        public void BuildDelivery(ModelBuilder modelBuilder)
        {
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
        }

    }
}