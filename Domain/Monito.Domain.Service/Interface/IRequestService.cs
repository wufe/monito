using System;
using Monito.Database.Entities;

namespace Monito.Domain.Service.Interface {
	public interface IRequestService {
		void Add(Request request);
		Request FindByGuid(Guid guid);
	}
}