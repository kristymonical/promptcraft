using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public class SVTContext : DbContext
    {
        public DbSet<Area> Areas { get; set; }
        public DbSet<DeliveryRequest> DeliveryRequests { get; set; }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Pool> Pools { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<TripHistory> TripHistory { get; set; }
        public DbSet<TripRequest> TripRequests { get; set; }

        public SVTContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Area/Pool many to many
            modelBuilder.Entity<AreaPool>(areaPool =>
            {
                areaPool
                    .HasKey(ap => new { ap.AreaId, ap.PoolId });
                areaPool
                    .HasOne(ap => ap.Area)
                    .WithMany(a => a.AreaPools)
                    .HasForeignKey(ap => ap.AreaId);
                areaPool
                    .HasOne(ap => ap.Pool)
                    .WithMany(p => p.AreaPools)
                    .HasForeignKey(ap => ap.PoolId);
            });

            // Area/Area many to many
            modelBuilder.Entity<AreaToArea>(areaToArea =>
            {
                areaToArea
                    .HasKey(ata => new { ata.FromAreaId, ata.ToAreaId });
                areaToArea
                    .HasOne(ata => ata.FromArea)
                    .WithMany(a => a.ToAreas)
                    .HasForeignKey(ata => ata.FromAreaId);
                areaToArea
                    .HasOne(ata => ata.ToArea)
                    .WithMany(a => a.FromAreas)
                    .HasForeignKey(ata => ata.FromAreaId);
            });

            // Position/TripRequest one to one
            modelBuilder.Entity<TripRequest>()
                .HasOne(t => t.Position)
                .WithOne(p => p.TripRequest)
                .HasForeignKey<Position>(p => p.TripRequestId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}