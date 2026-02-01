using Apo.Service.ShoppingCartAPI.Models.Dto;

namespace Apo.Service.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
