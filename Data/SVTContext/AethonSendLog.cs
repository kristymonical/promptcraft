using Microsoft.EntityFrameworkCore;
using SVT.Platform.Data.Models;

namespace SVT.Platform.Data
{
    public partial class SVTContext : DbContext
    {
        public DbSet<AethonSendLog> AethonSendLogs { get; set; }

        public void BuildAethonSendLog(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AethonSendLog>(aethonSendLogBuilder =>
            {
                aethonSendLogBuilder
                    .Property(d => d.InsertedBy)
                    .HasDefaultValueSql("suser_sname()");
                aethonSendLogBuilder
                    .Property(d => d.InsertedOn)
                    .HasDefaultValueSql("getdate()");
                aethonSendLogBuilder
                    .Property(d => d._Serialized)
                    .HasColumnName("Serialized");
            });
        }
    }
}