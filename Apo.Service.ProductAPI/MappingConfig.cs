using Apo.Service.ProductAPI.Models;
using Apo.Service.ProductAPI.Models.Dto;
using AutoMapper;

namespace Apo.Service.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMappings() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>().ReverseMap();
            });
;           return mappingConfig;
        }
    }
}
