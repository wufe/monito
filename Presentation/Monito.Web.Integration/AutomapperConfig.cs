using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Monito.Database.Entities;
using Monito.ValueObjects;
using Newtonsoft.Json;

namespace Monito.Web.Integration
{

    public class PresentationMappingProfile : Profile {
        public PresentationMappingProfile()
        {
            CreateMap<SaveJobInputModel, RequestOptions>();
            CreateMap<Request, RetrieveJobOutputModel>()
                .ForMember(d => d.Options, opt => opt.MapFrom(s => JsonConvert.DeserializeObject<RequestOptions>(s.Options)));
        }
    }

    public static class AutomapperConfigExtensions
    {
        public static void AddAutomapperConfigurations(this IServiceCollection services) {
            services
                .AddAutoMapper(cfg => {
                    cfg.AddProfile<PresentationMappingProfile>();
                }, typeof(AutomapperConfigExtensions));
        }
    }
}
