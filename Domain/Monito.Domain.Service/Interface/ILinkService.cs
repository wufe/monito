using System;
using System.Collections.Generic;
using System.Linq;
using Monito.Database.Entities;

namespace Monito.Domain.Service.Interface {
	public interface ILinkService {
		IQueryable<Link> GetDoneLinksAfterID(int linkID, int requestID);
	}
}