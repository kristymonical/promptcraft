using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public void BuildAreaDeliveryType(ModelBuilder modelBuilder)
        {
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
        }
    }
}