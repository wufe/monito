using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monito.Application.Model;
using Monito.Application.Model.Command;
using Monito.Application.Model.Query;
using Monito.Web.Extensions;
using Monito.Web.Models;
using Monito.Web.Services.Interface;

namespace Monito.Web.Controllers.API
{

    [ApiController]
	[Route("api/[controller]")]
	public class JobController : ControllerBase {
		private readonly IHttpRequestService _httpRequestService;
		private readonly ILogger<JobController> _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public JobController(
			IHttpRequestService httpRequestService,
			ILogger<JobController> logger,
			IMapper mapper,
			IMediator mediator
		)
		{
			_httpRequestService = httpRequestService;
			_logger             = logger;
			_mapper             = mapper;
			_mediator           = mediator;
		}

		[HttpPost("save")]
		public async Task<IActionResult> SaveJob([FromBody]SaveJobInputModel inputModel) {
			if (ModelState.IsValid) {

				var userIP = _httpRequestService.GetIP();
				var user = await _mediator.Send(UpsertUserByRequestIPCommand.Build(userIP));

				_logger.LogInformation("The user with IP " + userIP + " requested a job.");

				var requestUUID = await _mediator.Send(_mapper.Map<SaveJobInputModel, SaveJobCommand>(inputModel, SaveJobCommand.Build(userIP, user.ID)));

				return new JsonResult(new {
					userUUID = user.UUID,
					requestUUID = requestUUID
				});
			} else {
				// TODO: Return detailed error message
				return BadRequest();
			}
		}

		[HttpGet("{userUUID:guid}/{requestUUID:guid}")]
		public async Task<IActionResult> RetrieveJob(Guid userUUID, Guid requestUUID) {
			var request = await _mediator.Send(GetRequestByUUIDQuery.Build(requestUUID));
			
			if (request == null) {
				return NotFound();
			} else {
				return new JsonResult(request);
			}
		}

		[HttpGet("{userUUID:guid}/{requestUUID:guid}/status")]
		public async Task<IActionResult> RetrieveJobStatus(Guid userUUID, Guid requestUUID) {

			var request = await _mediator.Send(GetRequestStatusByUUIDQuery.Build(requestUUID));

			if (request == null) {
				return NotFound();
			} else {
				return new JsonResult(request);
			}
		}

		[HttpPost("{userUUID:guid}/{requestUUID:guid}/abort")]
		public async Task<IActionResult> AbortJob (Guid userUUID, Guid requestUUID){
			await _mediator.Send(AbortRequestCommand.Build(requestUUID));
			return Ok();
		}

		[HttpGet("{userUUID:guid}/{requestUUID:guid}/download/csv")]
		public async Task<IActionResult> DownloadJobAsCSV(Guid userUUID, Guid requestUUID)
		{
			var links = await _mediator.Send(GetRequestLinksByUUIDQuery.Build(requestUUID));
			if (links == null)
				return NotFound();

			return new FileCallbackResult(new MediaTypeHeaderValue("text/csv"), async (outputStream, _) =>
			{
				var writer = new StreamWriter(HttpContext.Response.Body);
				var csv = new CsvWriter(writer);
				csv.Configuration.RegisterClassMap<MinimalLinkApplicationModelCSVMap>();
				csv.WriteRecords(links);
				await writer.FlushAsync();
			})
			{
				FileDownloadName = "links.csv"
			};
		}
	}

}