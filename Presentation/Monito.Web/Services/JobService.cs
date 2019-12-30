using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Monito.Database.Entities;
using Monito.ValueObjects;
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
			var links = BuildLinksFromModel(inputModel);

			var options = BuildOptionsStringFromModel(inputModel);

			return new Request() {
				Links = links,
				Type = RequestType.Simple,
				Options = options,
				UserID = user.ID
			};
		}

		private ICollection<Link> BuildLinksFromModel(SaveJobInputModel inputModel) {
			return inputModel.Links.Split('\n')
				.Select(link => new Link() {
					OriginalURL = link.Trim()
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
	}
}