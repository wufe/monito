using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Monito.ValueObjects;

namespace Monito.Web.Integration
{

    public class PresentationMappingProfile : Profile {
        public PresentationMappingProfile()
        {
            CreateMap<SaveJobInputModel, RequestOptions>();
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
