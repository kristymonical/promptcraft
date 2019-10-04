using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<DeliveryType> DeliveryTypes { get; set; }

        public void BuildDeliveryType(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DeliveryType>(deliveryTypeBuilder =>
            {
                deliveryTypeBuilder
                    .HasData(
                        new DeliveryType{ Value = "deliver" },
                        new DeliveryType{ Value = "manual" },
                        new DeliveryType{ Value = "return" },
                        new DeliveryType{ Value = "stage" }
                    );
            });
        }
    }
}