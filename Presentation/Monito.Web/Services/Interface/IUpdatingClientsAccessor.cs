using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;

namespace Monito.Web.Services.Interface {
	public interface IUpdatingClientsAccessor {
		ICollection<JobClient> Clients { get; set; }
		void RegisterClient(HubCallerContext context, IClientProxy client, int requestID, int lastLinkID);
	}
}