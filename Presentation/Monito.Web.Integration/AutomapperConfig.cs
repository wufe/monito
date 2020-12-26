namespace Monito.Web.Integration
{

    // public class PresentationMappingProfile : Profile {
    //     public PresentationMappingProfile()
    //     {
    //         CreateMap<SaveJobInputModel, RequestOptions>();
    //         CreateMap<Request, RetrieveJobOutputModel>()
    //             .ForMember(d => d.Options, opt => opt.MapFrom(s => JsonConvert.DeserializeObject<RequestOptions>(s.Options)));
    //         CreateMap<Request, RetrieveJobStatusOutputModel>();
    //         CreateMap<Link, RetrieveBriefLinkOutputModel>()
    //             .ForMember(d => d.RedirectsToLinkId, opt =>
    //                 opt.MapFrom(s =>
    //                     s.RedirectsTo != null ? s.RedirectsTo.ID : default(int?)))
    //             .ForMember(d => d.URL, opt => opt.MapFrom(s => s.URL));
    //         CreateMap<Link, RetrieveLinkOutputModel>()
    //             .IncludeBase<Link, RetrieveBriefLinkOutputModel>();
    //     }
    // }

    // public static class AutomapperConfigExtensions
    // {
    //     public static void AddAutomapperConfigurations(this IServiceCollection services) {
    //         services
    //             .AddAutoMapper(cfg => {
    //                 cfg.AddProfile<PresentationMappingProfile>();
    //             }, typeof(AutomapperConfigExtensions));
    //     }
    // }
}
