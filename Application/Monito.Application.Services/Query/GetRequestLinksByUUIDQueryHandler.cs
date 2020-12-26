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
    public class GetRequestLinksByUUIDQueryHandler : IRequestHandler<GetRequestLinksByUUIDQuery, IEnumerable<MinimalLinkApplicationModel>>
    {
        private readonly IMapper _mapper;
        private readonly IReadRepository<RequestPersistenceModel> _requestRepository;
        private readonly IReadRepository<LinkPersistenceModel> _linkRepository;

        public GetRequestLinksByUUIDQueryHandler(
            IMapper mapper,
            IReadRepository<RequestPersistenceModel> requestRepository,
            IReadRepository<LinkPersistenceModel> linkRepository
        )
        {
            _mapper = mapper;
            _requestRepository = requestRepository;
            _linkRepository = linkRepository;
        }

        public Task<IEnumerable<MinimalLinkApplicationModel>> Handle(GetRequestLinksByUUIDQuery query, CancellationToken cancellationToken)
        {
            var requestPersistenceModel = _requestRepository
                .FindAll(x => x.UUID == query.UUID)
                .FirstOrDefault();

            if (requestPersistenceModel == null)
                return Task.FromResult<IEnumerable<MinimalLinkApplicationModel>>(null);

            IEnumerable<MinimalLinkApplicationModel> linksApplicationModels = _linkRepository
                .Include(x => x.RedirectsTo)
                .Where(x => x.RequestID == requestPersistenceModel.ID)
                .ProjectTo<MinimalLinkApplicationModel>(_mapper.ConfigurationProvider);

            return Task.FromResult(linksApplicationModels);
        }
    }
}