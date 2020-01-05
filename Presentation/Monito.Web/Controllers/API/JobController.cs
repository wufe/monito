using System;
using Microsoft.AspNetCore.Mvc;
using Monito.Domain.Service.Interface;
using Monito.ValueObjects;
using Monito.Web.Services.Interface;

namespace Monito.Web.Controllers.API {

	[ApiController]
	[Route("api/[controller]")]
	public class JobController : ControllerBase {
		private readonly IJobService _jobService;
		private readonly IHttpRequestService _httpRequestService;
		private readonly IRequestService _requestService;
		private readonly IUserService _userService;

		public JobController(
			IJobService jobService,
			IHttpRequestService httpRequestService,
			IRequestService requestService,
			IUserService userService
		)
		{
			_jobService         = jobService;
			_httpRequestService = httpRequestService;
			_requestService     = requestService;
			_userService        = userService;
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
				return new JsonResult(_jobService.BuildJobOutputModelFromRequest(request));
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
	}

}