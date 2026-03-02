using Apo.Services.PaymentAPI_V2.Application.Features.Payments;
using Apo.Services.PaymentAPI_V2.Domain;
using AutoMapper;

namespace Apo.Service.PaymentAPI_v2.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<PaymentRecord, PaymentDto>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
                config.CreateMap<PaymentDto, PaymentRecord>();
            });
            return mappingConfig;
        }
    }
}
