using Apo.Service.CouponAPI.Models;
using Apo.Service.CouponAPI.Models.Dto;
using AutoMapper;

namespace Apo.Service.CouponAPI
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
