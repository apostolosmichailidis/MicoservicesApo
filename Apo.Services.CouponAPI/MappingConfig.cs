using Apo.Services.CouponAPI.Models;
using Apo.Services.CouponAPI.Models.Dto;
using AutoMapper;

namespace Apo.Services.CouponAPI
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
