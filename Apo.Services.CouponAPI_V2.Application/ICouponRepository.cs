using Apo.Services.CouponAPI_V2.Domain;

namespace Apo.Services.CouponAPI_V2.Infrastructure
{
    public interface ICouponRepository
    {
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task<Coupon?> GetByIdAsync(int id);
        Task<Coupon?> GetByCodeAsync(string code);
        Task<Coupon> CreateAsync(Coupon coupon);
        Task<Coupon> UpdateAsync(Coupon coupon);
        Task<bool> DeleteAsync(int id);
    }
}
