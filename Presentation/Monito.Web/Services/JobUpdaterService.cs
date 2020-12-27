using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monito.Application.Model.Query;
using Monito.Web.Services.Interface;

namespace Monito.Web.Services
{
    public class JobUpdaterService : IJobUpdaterService, IHostedService {

		ILogger<IJobUpdaterService> _logger;
		IUpdatingClientsAccessor _clientsAccessor;
		private readonly IMapper _mapper;
		private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

		public JobUpdaterService(
			ILogger<IJobUpdaterService> logger,
			IUpdatingClientsAccessor clientsAccessor,
			IMapper mapper,
			IServiceScopeFactory serviceScopeFactory
		)
		{
			_clientsAccessor = clientsAccessor;
			_logger = logger;
			_mapper = mapper;
			_serviceScopeFactory = serviceScopeFactory;
		}

		public Task StartAsync(CancellationToken cancellationToken) {
			_logger.LogInformation("Starting job updater service.");
			_timer = new Timer(async _ => await Task.Run(SendUpdates), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			return Task.CompletedTask;
		}

		private async Task SendUpdates() {
			using (var scope = _serviceScopeFactory.CreateScope()) {

				var logger = scope.ServiceProvider.GetRequiredService<ILogger<IJobUpdaterService>>();
				var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

				foreach (var client in _clientsAccessor.Clients)
				{
					var links = await mediator.Send(GetDoneRequestLinksByRequestIDAfterIDQuery.Build(client.RequestID, client.LastLinkID));

					if (links.Count() > 0) {
						await client.Client.SendAsync("RetrieveUpdatedClients", links);
						client.LastLinkID = links.Last().ID;
					}
				}
			}
		}

		
	}

	
}