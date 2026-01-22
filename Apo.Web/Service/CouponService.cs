using Apo.Web.Models;
using Apo.Web.Service.IService;

namespace Apo.Web.Service
{
    public class CouponService : ICouponService
    {
        private IBaseService _baseService;

        public CouponService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateUpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.POST,
                Data = couponDto,
                Url = Apo.Web.Utility.SD.CouponAPIBase + $"/api/coupon"
            });
        }

        public async Task<ResponseDto?> DeleteCouponAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.DELETE,
                Url = Apo.Web.Utility.SD.CouponAPIBase + $"/api/coupon/{id}"
            });
        }

        public async Task<ResponseDto?> GetAllCouponsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.GET,
                Url = Apo.Web.Utility.SD.CouponAPIBase + "/api/coupon"
            });
        }

        public async Task<ResponseDto?> GetCoupnByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto() 
            {
                ApiType = Apo.Web.Utility.SD.ApiType.GET, 
                Url = Apo.Web.Utility.SD.CouponAPIBase + $"/api/coupon/{id}" 
            });
        }

        public async Task<ResponseDto?> GetCouponByCodeAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.GET,
                Url = Apo.Web.Utility.SD.CouponAPIBase + $"/api/coupon/GetByCode/{couponCode}"
            });
        }

        public async Task<ResponseDto?> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = Apo.Web.Utility.SD.ApiType.PUT,
                Data = couponDto,
                Url = Apo.Web.Utility.SD.CouponAPIBase + $"/api/coupon"
            });
        }
    }
}
