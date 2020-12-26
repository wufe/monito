using AutoMapper;
using AutoMapper.Extensions.EnumMapping;
using Monito.Application.Model;
using Monito.Persistence.Model;
using Newtonsoft.Json;

namespace Monito.Application.Configuration.Mapping
{
    public class PersistenceApplicationMappingProfile : Profile {
        public PersistenceApplicationMappingProfile()
        {
            CreateMap<UserPersistenceModel, MinimalUserApplicationModel>();
            CreateMap<LinkPersistenceModel, MinimalLinkApplicationModel>();
            CreateMap<RequestPersistenceModel, MinimalRequestApplicationModel>()
                .ForMember(d => d.Links, opt => opt.MapFrom(s => s.Links))
                .ForMember(d => d.Options, opt =>
                    opt.MapFrom(s =>
                        JsonConvert.DeserializeObject<RequestOptionsApplicationModel>(s.Options)));
            CreateMap<RequestPersistenceModel, MinimalRequestStatusApplicationModel>();
            CreateMap<RequestPersistenceModel, MinimalRequestWithDoneLinksCountApplicationModel>()
                .IncludeBase<RequestPersistenceModel, MinimalRequestApplicationModel>();
            CreateMap<LinkPersistenceModel, MinimalLinkApplicationModel>()
                .ForMember(d => d.RedirectsToLinkId, opt =>
                    opt.MapFrom(s =>
                        s.RedirectsTo != null ? s.RedirectsTo.ID : default(int?)));
            CreateMap<LinkPersistenceModel, LinkApplicationModel>()
                .IncludeBase<LinkPersistenceModel, MinimalLinkApplicationModel>();
        }
    }
}