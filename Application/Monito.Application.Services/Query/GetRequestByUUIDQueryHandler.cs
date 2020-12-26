using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Monito.Application.Model;
using Monito.Application.Model.Query;
using Monito.Persistence.Model;
using Monito.Persistence.Repository.Interface;

namespace Monito.Application.Services.Query
{
    public class GetRequestByUUIDQueryHandler : IRequestHandler<GetRequestByUUIDQuery, MinimalRequestWithDoneLinksCountApplicationModel>
    {
        private readonly IMapper _mapper;
        private readonly IReadRepository<RequestPersistenceModel> _requestRepository;
        private readonly IReadRepository<LinkPersistenceModel> _linkRepository;

        public GetRequestByUUIDQueryHandler(
            IMapper mapper,
            IReadRepository<RequestPersistenceModel> requestRepository,
            IReadRepository<LinkPersistenceModel> linkRepository
        )
        {
            _mapper = mapper;
            _requestRepository = requestRepository;
            _linkRepository = linkRepository;
        }

        public Task<MinimalRequestWithDoneLinksCountApplicationModel> Handle(GetRequestByUUIDQuery query, CancellationToken cancellationToken)
        {
            #region Find request persistence model
            var requestPersistenceModel = _requestRepository
                .FindAll(x => x.UUID == query.UUID)
                .FirstOrDefault();
            #endregion

            if (requestPersistenceModel == null)
                return Task.FromResult<MinimalRequestWithDoneLinksCountApplicationModel>(null);

            #region Find links
            var doneLinksCount = _linkRepository
                .FindAll(x => x.RequestID == requestPersistenceModel.ID && x.Status == LinkStatus.Done) // TODO: Add link status filter in command
                .Count();
            var linksPersistenceModels = _linkRepository
                .FindAll(x => x.RequestID == requestPersistenceModel.ID && x.Status == LinkStatus.Done) // TODO: Add link status filter in command
                .OrderBy(x => x.ID)
                .Take(2000) // TODO: Limit links in command
                .ToList();
            #endregion

            #region Convert request to application model
            var requestApplicationModel = _mapper.Map<MinimalRequestWithDoneLinksCountApplicationModel>(requestPersistenceModel);
            #endregion

            #region Convert links to application models
            var linksApplicationModels = _mapper.Map<ICollection<MinimalLinkApplicationModel>>(linksPersistenceModels);
            #endregion

            requestApplicationModel.Links = linksApplicationModels;
            requestApplicationModel.DoneLinksCount = doneLinksCount;
            return Task.FromResult(requestApplicationModel);
        }
    }
}