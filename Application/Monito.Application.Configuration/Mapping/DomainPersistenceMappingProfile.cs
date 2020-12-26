using AutoMapper;
using Monito.Domain.Entity;
using Monito.Persistence.Model;
using Newtonsoft.Json;

namespace Monito.Application.Configuration.Mapping
{
    public class DomainPersistenceMappingProfile : Profile {
        public DomainPersistenceMappingProfile()
        {
            CreateMap<FilePersistenceModel, FileDomainEntity>()
                .ReverseMap();
            CreateMap<LinkPersistenceModel, LinkDomainEntity>()
                .ReverseMap();
            CreateMap<QueuePersistenceModel, QueueDomainEntity>()
                .ReverseMap();
            CreateMap<RequestPersistenceModel, RequestDomainEntity>()
                .ForMember(
                    d => d.Options,
                    opt => opt.MapFrom(s =>
                        JsonConvert.DeserializeObject<RequestOptionsDomainEntity>(s.Options)))
                .ReverseMap()
                .ForMember(
                    d => d.Options,
                    opt => opt.MapFrom(s => 
                        JsonConvert.SerializeObject(s.Options))
                );
            CreateMap<UserPersistenceModel, UserDomainEntity>()
                .ReverseMap();
            CreateMap<WorkerPersistenceModel, WorkerDomainEntity>()
                .ReverseMap();
            
        }
    }
}