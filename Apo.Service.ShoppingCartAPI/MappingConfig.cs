using Apo.Service.ShoppingCartAPI.Models;
using Apo.Service.ShoppingCartAPI.Models.Dto;
using AutoMapper;

namespace Apo.Service.ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMappings() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap();
            });
;           return mappingConfig;
        }
    }
}
