using System;
using System.Collections.Generic;
using System.Linq;
using Monito.Database.Entities;

namespace Monito.Domain.Service.Interface {
	public interface IRequestService {
		void Add(Request request);
		Request FindByGuid(Guid guid, bool limitLinks = true);
		int GetLinksCountByRequestId(int ID, bool doneOnly = true);
		IQueryable<Link> GetAllLinksByRequestID(int ID);
		void Abort(Request request);
	}
}