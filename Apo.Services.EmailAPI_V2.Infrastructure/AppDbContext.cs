using Apo.Services.EmailAPI_V2.Domain;
using Microsoft.EntityFrameworkCore;

namespace Apo.Services.EmailAPI_V2.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EmailRecord> EmailRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmailRecord>()
                .Property(e => e.Status)
                .HasConversion<string>();
        }
    }
}
