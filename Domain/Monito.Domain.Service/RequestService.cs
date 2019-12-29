using Monito.Database.Entities;
using Monito.Domain.Service.Interface;
using Monito.Repository.Interface;

namespace Monito.Domain.Service {
	public class RequestService : IRequestService {
		private readonly IRepository<Request> _requestRepository;

		public RequestService(IRepository<Request> requestRepository)
		{
			_requestRepository = requestRepository;
		}

		// TODO: Limit number of pending requests per user
		public void AddRequest(Request request)
		{
			_requestRepository.Insert(request);
			_requestRepository.SaveChanges();
		}
	}
}