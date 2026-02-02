using Apo.Services.CouponAPI_V2.Domain;
using Microsoft.EntityFrameworkCore;

namespace Apo.Services.CouponAPI_V2.Infrastructure
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _db;

        public CouponRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
            => await _db.Coupons.ToListAsync();

        public async Task<Coupon?> GetByIdAsync(int id)
            => await _db.Coupons.FirstOrDefaultAsync(c => c.CouponId == id);

        public async Task<Coupon?> GetByCodeAsync(string code)
            => await _db.Coupons.FirstOrDefaultAsync(c => c.CouponCode.ToLower() == code.ToLower());

        public async Task<Coupon> CreateAsync(Coupon coupon)
        {
            _db.Coupons.Add(coupon);
            await _db.SaveChangesAsync();
            return coupon;
        }

        public async Task<Coupon> UpdateAsync(Coupon coupon)
        {
            _db.Coupons.Update(coupon);
            await _db.SaveChangesAsync();
            return coupon;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var coupon = await GetByIdAsync(id);
            if (coupon == null) return false;

            _db.Coupons.Remove(coupon);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
