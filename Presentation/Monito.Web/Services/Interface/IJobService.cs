using Monito.Database.Entities;
using Monito.ValueObjects;

namespace Monito.Web.Services.Interface {
	public interface IJobService {
		Request BuildRequest(SaveJobInputModel inputModel, User user);
	}
}