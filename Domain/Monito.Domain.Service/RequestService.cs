using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AutoMapper;
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

		public Request FindByGuid(Guid guid, bool limitLinks = true) {
			var request = _requestRepository
				.FindAll(x => x.UUID == guid)
				.FirstOrDefault();

			if (request != null && limitLinks) {
				request.Links = _linksRepository
					.FindAll(x => x.RequestID == request.ID && x.Status == LinkStatus.Done)
					.OrderBy(x => x.ID)
					.Take(2000)
					.ToList();
			}

			return request;
		}

		public int GetLinksCountByRequestId(int ID, bool doneOnly = true)
		{
			var links = _linksRepository
				.FindAll(x => x.RequestID == ID);
			if (doneOnly)
				links = links.Where(x => x.Status == LinkStatus.Done);
			return links.Count();
		}

		public IQueryable<Link> GetAllLinksByRequestID(int ID) {
			return _linksRepository
				.AsNoTracking()
				.Where(x => x.RequestID == ID);
		}
	}
}