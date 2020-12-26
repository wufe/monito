using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Monito.Application.Model;
using Monito.Application.Model.Command;
using Monito.Domain.Entity;
using Monito.Persistence.Model;
using Monito.Persistence.Repository.Interface;

namespace Monito.Application.Services.Command
{
    public class UpsertUserByRequestIPCommandHandler : IRequestHandler<UpsertUserByRequestIPCommand, MinimalUserApplicationModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<UserPersistenceModel> _userRepository;

        public UpsertUserByRequestIPCommandHandler(
            IMapper mapper,
            IRepository<UserPersistenceModel> userRepository
        )
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public Task<MinimalUserApplicationModel> Handle(UpsertUserByRequestIPCommand command, CancellationToken cancellationToken)
        {
            #region Finding user
            var userPersistenceModel = _userRepository
                .FindAll(x => x.IP == command.RequestIP)
                .FirstOrDefault();
            #endregion

            #region Creating user
            if (userPersistenceModel == null) {

                #region Creating user domain entity
                var userDomainEntity = UserDomainEntity.Build(command.RequestIP);
                #endregion

                #region Creating user persistence model
                userPersistenceModel = _mapper.Map<UserPersistenceModel>(userDomainEntity);
                #endregion

                #region Storing user
                _userRepository.Insert(userPersistenceModel);
                #endregion

            }
            #endregion

            #region Mapping user to application model
            var userApplicationModel = _mapper.Map<MinimalUserApplicationModel>(userPersistenceModel);
            #endregion

            return Task.FromResult(userApplicationModel);
        }
    }
}