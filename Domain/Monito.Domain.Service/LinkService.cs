using System;
using System.Collections.Generic;
using System.Linq;
using Monito.Database.Entities;
using Monito.Domain.Service.Interface;
using Monito.Repository.Interface;

namespace Monito.Domain.Service {
	public class LinkService : ILinkService
	{
		private readonly IReadRepository<Link> _linkRepository;

		public LinkService(IReadRepository<Link> linkRepository)
		{
			_linkRepository = linkRepository;
		}

		public IQueryable<Link> GetDoneLinksAfterID(int linkID, int requestID) =>
			_linkRepository
				.FindAll(x => x.RequestID == requestID && x.ID > linkID && x.Status == LinkStatus.Done)
				.OrderBy(x => x.ID)
				.Take(100);
	}
}