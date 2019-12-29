using Monito.Database.Entities;
using Monito.Web.Models.Input;

namespace Monito.Web.Services.Interface {
	public interface IJobService {
		Request BuildRequest(SaveJobInputModel inputModel, User user);
	}
}