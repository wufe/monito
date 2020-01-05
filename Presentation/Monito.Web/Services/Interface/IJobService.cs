using Monito.Database.Entities;
using Monito.ValueObjects;
using Monito.ValueObjects.Output;

namespace Monito.Web.Services.Interface {
	public interface IJobService {
		Request BuildRequest(SaveJobInputModel inputModel, User user);
		RetrieveJobOutputModel BuildJobOutputModelFromRequest(Request request);
		RetrieveJobStatusOutputModel BuildJobStatusOutputModelFromRequest(Request request);
	}
}