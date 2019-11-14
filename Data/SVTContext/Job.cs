using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }

        public void BuildJob(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Job>(jobBuilder =>
            {
                jobBuilder
                    .Property(d => d.InsertedBy)
                    .HasDefaultValueSql("suser_sname()");
                jobBuilder
                    .Property(d => d.InsertedOn)
                    .HasDefaultValueSql("getutcdate()");
                jobBuilder
                    .Property(d => d.ModifiedBy)
                    .HasDefaultValueSql("suser_sname()");
                jobBuilder
                    .Property(d => d.ModifiedOn)
                    .HasDefaultValueSql("getutcdate()");
                jobBuilder
                    .HasOne(job => job.Delivery)
                    .WithMany(delivery => delivery.Jobs)
                    .HasForeignKey(job => job.DeliveryId);
            });
        }
    }
}