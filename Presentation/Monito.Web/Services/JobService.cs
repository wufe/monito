using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Monito.Database.Entities;
using Monito.ValueObjects;
using Monito.ValueObjects.Output;
using Monito.Web.Services.Interface;
using Newtonsoft.Json;

namespace Monito.Web.Services {
	public class JobService : IJobService
	{
		private readonly IMapper _mapper;

		public JobService(IMapper mapper)
		{
			_mapper = mapper;
		}

		public Request BuildRequest(SaveJobInputModel inputModel, User user)
		{
			var options = BuildOptionsStringFromModel(inputModel);

			var links = BuildLinksFromJobModel(inputModel);

			return new Request() {
				Links = links,
				Type = RequestType.Simple,
				Options = options,
				UserID = user.ID
			};
		}

		private ICollection<Link> BuildLinksFromJobModel(SaveJobInputModel inputModel) {
			return (inputModel.Links ?? "")
				.Split('\n')
				.Select(x => new Link() {
					Status = LinkStatus.Idle,
					URL = x.Trim()
				})
				.ToList();
		}

		private string BuildOptionsStringFromModel(SaveJobInputModel inputModel) {
			var optionsValueObject = _mapper.Map<SaveJobInputModel, RequestOptions>(inputModel);
			return JsonConvert.SerializeObject(optionsValueObject);
		}

		public RetrieveJobOutputModel BuildJobOutputModelFromRequest(Request request) {
			return _mapper.Map<Request, RetrieveJobOutputModel>(request);
		}

		public RetrieveJobStatusOutputModel BuildJobStatusOutputModelFromRequest(Request request)
		{
			return _mapper.Map<Request, RetrieveJobStatusOutputModel>(request);
		}
	}
}