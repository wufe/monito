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
    public class GetRequestStatusByUUIDQueryHandler : IRequestHandler<GetRequestStatusByUUIDQuery, MinimalRequestStatusApplicationModel>
    {
        private readonly IMapper _mapper;
        private readonly IReadRepository<RequestPersistenceModel> _requestRepository;

        public GetRequestStatusByUUIDQueryHandler(
            IMapper mapper,
            IReadRepository<RequestPersistenceModel> requestRepository
        )
        {
            _mapper = mapper;
            _requestRepository = requestRepository;
        }

        Task<MinimalRequestStatusApplicationModel> IRequestHandler<GetRequestStatusByUUIDQuery, MinimalRequestStatusApplicationModel>.Handle(GetRequestStatusByUUIDQuery query, CancellationToken cancellationToken)
        {
            var requestPersistenceModel = _requestRepository
                .FindAll(x => x.UUID == query.UUID)
                .FirstOrDefault();

            if (requestPersistenceModel == null)
                return Task.FromResult<MinimalRequestStatusApplicationModel>(null);

            var requestStatusApplicationModel = _mapper.Map<MinimalRequestStatusApplicationModel>(requestPersistenceModel);

            return Task.FromResult(requestStatusApplicationModel);
        }
    }
}