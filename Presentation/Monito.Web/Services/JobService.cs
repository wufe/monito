using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Monito.Database.Entities;
using Monito.Domain.Service.Interface;
using Monito.ValueObjects;
using Monito.ValueObjects.Output;
using Monito.Web.Services.Interface;
using Newtonsoft.Json;

namespace Monito.Web.Services {
	public class JobService : IJobService
	{
		private readonly IMapper _mapper;
		private readonly IRequestService _requestService;

		public JobService(
			IMapper mapper,
			IRequestService requestService
		)
		{
			_mapper = mapper;
			_requestService = requestService;
		}

		public Request BuildRequest(SaveJobInputModel inputModel, User user)
		{
			var options = BuildOptionsStringFromModel(inputModel);

			var links = BuildLinksFromJobModel(inputModel);

			return new Request() {
				IP      = user.IP,
				Links   = links,
				Type    = RequestType.Simple,
				Options = options,
				UserID  = user.ID,
				Status  = RequestStatus.Ready
			};
		}

		private ICollection<Link> BuildLinksFromJobModel(SaveJobInputModel inputModel) {
			return (inputModel.Links ?? "")
				.Split('\n')
				.Select(x => new Link() {
					Status = LinkStatus.Idle,
					URL = x.Trim()
				})
				.Where(x => !string.IsNullOrWhiteSpace(x.URL))
				.ToList();
		}

		private string BuildOptionsStringFromModel(SaveJobInputModel inputModel) {
			var optionsValueObject = _mapper.Map<SaveJobInputModel, RequestOptions>(inputModel);
			return JsonConvert.SerializeObject(optionsValueObject);
		}

		public RetrieveJobOutputModel BuildJobOutputModelFromRequest(Request request, int linksCount = -1) {
			var outputModel = _mapper.Map<Request, RetrieveJobOutputModel>(request);
			if (linksCount > -1) {
				outputModel.LinksCount = linksCount;
			}
			return outputModel;
		}

		public RetrieveJobStatusOutputModel BuildJobStatusOutputModelFromRequest(Request request)
		{
			return _mapper.Map<Request, RetrieveJobStatusOutputModel>(request);
		}

		public IEnumerable<RetrieveBriefLinkOutputModel> GetLinksForDownloadByRequestID(int ID) {
			return _requestService
				.GetAllLinksByRequestID(ID)
				/*.ToList()
				.AsQueryable()*/
				.ProjectTo<RetrieveBriefLinkOutputModel>(_mapper.ConfigurationProvider);
		}
	}
}