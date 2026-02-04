using Apo.Services.CouponAPI_V2.Application.Features.Coupons;
using Apo.Services.CouponAPI_V2.Domain;
using AutoMapper;

namespace Apo.Service.CouponAPI_v2.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMappings() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDto>();
                config.CreateMap<CouponDto, Coupon>();
            });
;           return mappingConfig;
        }
    }
}
