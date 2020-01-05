using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Monito.Web.Services.Interface;

namespace Monito.Web.Hubs {
	public class JobHub : Hub {
		private readonly ILogger<JobHub> _logger;
		private readonly IJobUpdaterService _jobUpdaterService;
		private readonly IUpdatingClientsAccessor _updatingClientsAccessor;

		public JobHub(
			ILogger<JobHub> logger,
			IJobUpdaterService jobUpdaterService,
			IUpdatingClientsAccessor updatingClientsAccessor)
		{
			_logger = logger;
			_jobUpdaterService = jobUpdaterService;
			_updatingClientsAccessor = updatingClientsAccessor;
		}

		public void RequestJobUpdates(int requestID, int lastLinkID) {
			_updatingClientsAccessor.RegisterClient(Context, Clients.Caller, requestID, lastLinkID);
		}
	}
}