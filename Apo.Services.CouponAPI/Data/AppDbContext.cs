using Apo.Service.CouponAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Apo.Service.CouponAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Coupon>().HasData(
                new Coupon
                {
                    CouponId = 1,
                    CouponCode = "WELCOME10",
                    DiscountAmount = 10,
                    MinAmmount = 50,
                },
                new Coupon
                {
                    CouponId = 2,
                    CouponCode = "SUMMER20",
                    DiscountAmount = 20,
                    MinAmmount = 100,
                }
            );
        }
    }
}
