using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<Itinerary> Itineraries { get; set; }

        public void BuildItinerary(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Itinerary>(itineraryBuilder =>
            {
                itineraryBuilder
                    .Property(d => d.InsertedBy)
                    .HasDefaultValueSql("suser_sname()");
                itineraryBuilder
                    .Property(d => d.InsertedOn)
                    .HasDefaultValueSql("getutcdate()");
                itineraryBuilder
                    .Property(d => d.ModifiedBy)
                    .HasDefaultValueSql("suser_sname()");
                itineraryBuilder
                    .Property(d => d.ModifiedOn)
                    .HasDefaultValueSql("getutcdate()");
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
        }
    }
}