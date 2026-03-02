using Apo.Services.EmailAPI_V2.Application.Features.Emails;
using Apo.Services.EmailAPI_V2.Domain;
using AutoMapper;

namespace Apo.Service.EmailAPI_v2.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMappings()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<EmailRecord, EmailDto>()
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
                config.CreateMap<EmailDto, EmailRecord>();
            });
            return mappingConfig;
        }
    }
}
