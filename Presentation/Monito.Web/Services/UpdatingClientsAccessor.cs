using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Monito.Web.Services.Interface;

namespace Monito.Web.Services {
	public class UpdatingClientsAccessor : IUpdatingClientsAccessor {
		public ICollection<JobClient> Clients { get; set; } = new List<JobClient>();
		private readonly ILogger<IUpdatingClientsAccessor> _logger;

		public UpdatingClientsAccessor(ILogger<IUpdatingClientsAccessor> logger)
		{
			_logger = logger;
		}

		public void RegisterClient(HubCallerContext context, IClientProxy client, int requestID, int lastLinkID) {
			var connectionID = context.ConnectionId;
			var connectionAborted = context.ConnectionAborted;

			var foundJobClient = Clients
				.FirstOrDefault(x => x.ConnectionID == connectionID);
			if (foundJobClient != null)
				return;

			connectionAborted.Register(() => {
				RemoveClientByConnectId(connectionID);
			});

			var newClient = new JobClient() {
				Client = client,
				ConnectionID = connectionID,
				RequestID = requestID,
				LastLinkID = lastLinkID
			};
			
			Clients.Add(newClient);
			
		}

		private void RemoveClientByConnectId(string connectionID) {
			Clients = Clients.Where(x => x.ConnectionID != connectionID)
				.ToList();
		}
	}

	public class JobClient {
		public string ConnectionID { get; set; }
		public IClientProxy Client { get; set; }
		public int RequestID { get; set; }
		public int LastLinkID { get; set; }
	}
}