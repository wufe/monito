using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monito.Database.Entities;
using Monito.Domain.Service.Interface;
using Monito.ValueObjects;
using Monito.Web.Extensions;
using Monito.Web.Models;
using Monito.Web.Services.Interface;

namespace Monito.Web.Controllers.API {

	[ApiController]
	[Route("api/[controller]")]
	public class JobController : ControllerBase {
		private readonly IJobService _jobService;
		private readonly IHttpRequestService _httpRequestService;
		private readonly IRequestService _requestService;
		private readonly IUserService _userService;
		private readonly ILogger<JobController> _logger;

		public JobController(
			IJobService jobService,
			IHttpRequestService httpRequestService,
			IRequestService requestService,
			IUserService userService,
			ILogger<JobController> logger
		)
		{
			_jobService         = jobService;
			_httpRequestService = httpRequestService;
			_requestService     = requestService;
			_userService        = userService;
			_logger             = logger;
		}

		[HttpPost("save")]
		public IActionResult SaveJob([FromBody]SaveJobInputModel inputModel) {
			if (ModelState.IsValid) {
				var user = _userService.FindOrCreateUserByIP(_httpRequestService.GetIP());
				var request = _jobService.BuildRequest(inputModel, user);
				_requestService.Add(request);
				return new JsonResult(new {
					userUUID = user.UUID,
					requestUUID = request.UUID
				});
			} else {
				// TODO: Return detailed error message
				return BadRequest();
			}
		}

		[HttpGet("{userUUID:guid}/{requestUUID:guid}")]
		public IActionResult RetrieveJob(Guid userUUID, Guid requestUUID) {
			var request = _requestService.FindByGuid(requestUUID);
			if (request == null) {
				return NotFound();
			} else {
				var linksCount = _requestService.GetLinksCountByRequestId(request.ID);
				return new JsonResult(_jobService.BuildJobOutputModelFromRequest(request, linksCount));
			}
		}

		[HttpGet("{userUUID:guid}/{requestUUID:guid}/status")]
		public IActionResult RetrieveJobStatus(Guid userUUID, Guid requestUUID) {
			var request = _requestService.FindByGuid(requestUUID);
			if (request == null) {
				return NotFound();
			} else {
				return new JsonResult(_jobService.BuildJobStatusOutputModelFromRequest(request));
			}
		}

		[HttpGet("{userUUID:guid}/{requestUUID:guid}/download/csv")]
		public IActionResult DownloadJobAsCSV(Guid userUUID, Guid requestUUID)
		{
			var request = _requestService.FindByGuid(requestUUID, false);
			if (request == null || request.Status != RequestStatus.Done)
			{
				return NotFound();
			}

			var links = _jobService.GetLinksForDownloadByRequestID(request.ID);

			return new FileCallbackResult(new MediaTypeHeaderValue("text/csv"), async (outputStream, _) =>
			{
				var writer = new StreamWriter(HttpContext.Response.Body);
				var csv = new CsvWriter(writer);
				csv.Configuration.RegisterClassMap<RetrieveBriefLinkOutputModelCSVMap>();
				csv.WriteRecords(links);
				await writer.FlushAsync();
			})
			{
				FileDownloadName = "links.csv"
			};
		}
	}

}