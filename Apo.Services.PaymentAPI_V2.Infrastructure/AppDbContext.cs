using Apo.Services.PaymentAPI_V2.Domain;
using Microsoft.EntityFrameworkCore;

namespace Apo.Services.PaymentAPI_V2.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<PaymentRecord> PaymentRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PaymentRecord>()
                .Property(p => p.Status)
                .HasConversion<string>();
        }
    }
}
