using Apo.Services.ProductAPI_V2.Application.Features.Products;
using Apo.Services.ProductAPI_V2.Domain;
using AutoMapper;

namespace Apo.Service.ProductAPI_v2.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMappings() 
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Product, ProductDto>();
                config.CreateMap<ProductDto, Product>();
            });
;           return mappingConfig;
        }
    }
}
