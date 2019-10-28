using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<Log> Logs { get; set; }

        public void BuildLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>(logBuilder =>
            {
                logBuilder
                    .Property(d => d.InsertedBy)
                    .HasDefaultValueSql("suser_sname()");
                logBuilder
                    .Property(d => d.InsertedOn)
                    .HasDefaultValueSql("getdate()");
                logBuilder
                    .Property(d => d.ModifiedBy)
                    .HasDefaultValueSql("suser_sname()");
                logBuilder
                    .Property(d => d.ModifiedOn)
                    .HasDefaultValueSql("getdate()");
                logBuilder
                    .HasOne(log => log.Delivery)
                    .WithMany(delivery => delivery.Logs)
                    .HasForeignKey(log => log.DeliveryId)
                    .IsRequired(false);
                logBuilder
                    .HasOne(log => log.ActionTypeReference)
                    .WithMany(actionType => actionType.Logs)
                    .HasForeignKey(log => log.Action);
            });
        }
    }
}