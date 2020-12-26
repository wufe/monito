using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Monito.Application.Model.Command;
using Monito.Domain.Entity;
using Monito.Persistence.Model;
using Monito.Persistence.Repository.Interface;

namespace Monito.Application.Services.Command
{
    public class AbortRequestCommandHandler : IRequestHandler<AbortRequestCommand>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<RequestPersistenceModel> _requestRepository;

        public AbortRequestCommandHandler(
            IMapper mapper,
            IRepository<RequestPersistenceModel> requestRepository
        )
        {
            _mapper = mapper;
            _requestRepository = requestRepository;
        }

        public Task<Unit> Handle(AbortRequestCommand query, CancellationToken cancellationToken)
        {
            var requestPersistenceModel = _requestRepository
                .FindAll(x => x.UUID == query.UUID)
                .FirstOrDefault();

            if (requestPersistenceModel == null)
                return Task.FromResult(new Unit()); // TODO: Add "Not Found" result

            var requestDomainEntity = _mapper.Map<RequestDomainEntity>(requestPersistenceModel);
            requestDomainEntity.Abort();

            requestPersistenceModel = _mapper.Map<RequestDomainEntity, RequestPersistenceModel>(requestDomainEntity, requestPersistenceModel);

            _requestRepository.Update(requestPersistenceModel);
            
            return Task.FromResult(new Unit());
        }
    }
}