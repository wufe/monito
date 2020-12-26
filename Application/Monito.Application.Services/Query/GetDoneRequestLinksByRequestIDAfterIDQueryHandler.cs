using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Monito.Application.Model;
using Monito.Application.Model.Query;
using Monito.Persistence.Model;
using Monito.Persistence.Repository.Interface;

namespace Monito.Application.Services.Query
{
    public class GetDoneRequestLinksByRequestIDAfterIDQueryHandler : IRequestHandler<GetDoneRequestLinksByRequestIDAfterIDQuery, IEnumerable<MinimalLinkApplicationModel>>
    {
        private readonly IMapper _mapper;
        private readonly IReadRepository<LinkPersistenceModel> _linkRepository;

        public GetDoneRequestLinksByRequestIDAfterIDQueryHandler(
            IMapper mapper,
            IReadRepository<LinkPersistenceModel> linkRepository
        )
        {
            _mapper = mapper;
            _linkRepository = linkRepository;
        }

        public Task<IEnumerable<MinimalLinkApplicationModel>> Handle(GetDoneRequestLinksByRequestIDAfterIDQuery query, CancellationToken cancellationToken)
        {
            IEnumerable<MinimalLinkApplicationModel> linksApplicationModels = _linkRepository
                .FindAll(x => x.RequestID == query.RequestID && x.ID > query.LinkID && x.Status == LinkStatus.Done)
                .OrderBy(x => x.ID)
                .Take(100)
                .ProjectTo<MinimalLinkApplicationModel>(_mapper.ConfigurationProvider)
                .ToList();

            return Task.FromResult(linksApplicationModels);
        }
    }
}