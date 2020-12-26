using Monito.Web.Services.Interface;

namespace Monito.Web.Services
{
    public class JobService : IJobService
	{
        // private readonly IMapper _mapper;
        // private readonly IRequestService _requestService;
        // private readonly IPerformanceService _performanceService;

        // public JobService(
        // 	IMapper mapper,
        // 	IRequestService requestService,
        // 	IPerformanceService performanceService
        // )
        // {
        // 	_mapper = mapper;
        // 	_requestService = requestService;
        // 	_performanceService = performanceService;
        // }

        // TODO: Delete
        // public Request BuildRequest(SaveJobInputModel inputModel, User user)
        // {
        // 	var options = BuildOptionsStringFromModel(inputModel);

        // 	ICollection<Link> links;
        // 	using (_performanceService.Start("BuildLinksFromJobModel"))
        // 		links = BuildLinksFromJobModel(inputModel);

        // 	return new Request() {
        // 		IP      = user.IP,
        // 		Links   = links,
        // 		Type    = RequestType.Simple,
        // 		Options = options,
        // 		UserID  = user.ID,
        // 		Status  = RequestStatus.Ready
        // 	};
        // }

        // TODO: Delete
        // private Uri ParseURI(string url) {
        // 	if (!url.StartsWith("http"))
        // 		url = "http://" + url;
        // 	try {
        // 		return new Uri(url);
        // 	}  catch (Exception) {
        // 		return null;
        // 	}
        // }

        // TODO: Delete
        // private ICollection<Link> BuildLinksFromJobModel(SaveJobInputModel inputModel) {

        // 	var rawLinks = (inputModel.Links ?? "")
        // 		.Split('\n')
        // 		.Select(x => new IntermediateLink() {
        // 			Status = LinkStatus.Idle,
        // 			URL = x.Trim(),
        // 			URI = ParseURI(x.Trim())
        // 		})
        // 		.Where(x => !string.IsNullOrWhiteSpace(x.URL) && x.URI != null);

        // 	var linksGroupedByHost = rawLinks
        // 		.GroupBy(x => x.URI.Host);

        // 	var hosts = linksGroupedByHost
        // 		.ToDictionary(x => x.Key, x => x.ToList());

        // 	var hostsEmpty = false;
        // 	var index = 0;

        // 	var roundedLinks = new List<IntermediateLink>();

        // 	while (!hostsEmpty) {
        // 		hostsEmpty = true;
        // 		foreach(var keyValue in hosts) {
        // 			if (keyValue.Value.Count > index) {
        // 				hostsEmpty = false;
        // 				roundedLinks.Add(keyValue.Value.ElementAt(index));
        // 			}
        // 		}
        // 		index++;
        // 	}

        // 	return roundedLinks
        // 		.Select(x => new Link() {
        // 			Status = x.Status,
        // 			URL = x.URL
        // 		})
        // 		.ToList();
        // }

        // private string BuildOptionsStringFromModel(SaveJobInputModel inputModel) {
        // 	var optionsValueObject = _mapper.Map<SaveJobInputModel, RequestOptions>(inputModel);
        // 	return JsonConvert.SerializeObject(optionsValueObject);
        // }

        // TODO: Delete
        // public RetrieveJobOutputModel BuildJobOutputModelFromRequest(Request request, int linksCount = -1) {
        // 	var outputModel = _mapper.Map<Request, RetrieveJobOutputModel>(request);
        // 	if (linksCount > -1) {
        // 		outputModel.LinksCount = linksCount;
        // 	}
        // 	return outputModel;
        // }

        // TODO: Delete
        // public RetrieveJobStatusOutputModel BuildJobStatusOutputModelFromRequest(Request request)
        // {
        // 	return _mapper.Map<Request, RetrieveJobStatusOutputModel>(request);
        // }

        // public IEnumerable<RetrieveBriefLinkOutputModel> GetLinksForDownloadByRequestID(int ID) {
        // 	return _requestService
        // 		.GetAllLinksByRequestID(ID)
        // 		.ProjectTo<RetrieveBriefLinkOutputModel>(_mapper.ConfigurationProvider);
        // }

        // TODO: Delete
        // private class IntermediateLink : Link {
        // 	public Uri URI { get; set; }
        // }
    }
}