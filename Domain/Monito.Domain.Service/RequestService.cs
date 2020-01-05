using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Monito.Database.Entities;
using Monito.Domain.Service.Interface;
using Monito.Repository.Interface;

namespace Monito.Domain.Service {
	public class RequestService : IRequestService {
		private readonly IRepository<Request> _requestRepository;
		private readonly IMapper _mapper;

		public RequestService(
			IRepository<Request> requestRepository,
			IMapper mapper)
		{
			_requestRepository = requestRepository;
			_mapper = mapper;
		}

		// TODO: Limit number of pending requests per user
		public void Add(Request request)
		{
			_requestRepository.Insert(request);
			_requestRepository.SaveChanges();
		}

		public Request FindByGuid(Guid guid) {
			var request = _requestRepository
				.FindAll()
				.ProjectTo<RequestWithDoneLinks>(_mapper.ConfigurationProvider)
				.FirstOrDefault(x => x.UUID == guid);
			return request;
		}
	}
}