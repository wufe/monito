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
		private readonly IRepository<Link> _linksRepository;
		private readonly IMapper _mapper;

		public RequestService(
			IRepository<Request> requestRepository,
			IRepository<Link> linksRepository,
			IMapper mapper)
		{
			_requestRepository = requestRepository;
			_linksRepository = linksRepository;
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
				.FirstOrDefault(x => x.UUID == guid);

			if (request != null) {
				request.Links = _linksRepository
					.FindAll(x => x.RequestID == request.ID && x.Status == LinkStatus.Done)
					.OrderBy(x => x.ID)
					.Take(100000)
					.ToList();
			}

			return request;
		}
	}
}