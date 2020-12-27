using AutoMapper;
using Monito.Application.Model.Command;
using Monito.Web.Models;

namespace Monito.Web.Configuration.Mapping
{
    public class PresentationApplicationMappingProfile : Profile {
        public PresentationApplicationMappingProfile()
        {
            CreateMap<SaveJobInputModel, SaveJobCommand>();
        }
    }
}