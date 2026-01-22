using Apo.Web.Models;

namespace Apo.Web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto?> GetCouponByCodeAsync(string couponCode);
        Task<ResponseDto?> GetCoupnByIdAsync(int id);
        Task<ResponseDto?> GetAllCouponsAsync();
        Task<ResponseDto?> CreateUpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDto?> DeleteCouponAsync(int id);
    }
}
