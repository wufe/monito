using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Monito.Database.Entities;
using Monito.Domain.Service.Interface;
using Monito.ValueObjects.Output;
using Monito.Web.Services.Interface;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;

namespace Monito.Web.Services {
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
			_timer = new Timer(async _ => await Task.Run(SendUpdates), null, TimeSpan.Zero, TimeSpan.FromSeconds(3));
			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken) {
			return Task.CompletedTask;
		}

		private async Task SendUpdates() {
			using (var scope = _serviceScopeFactory.CreateScope()) {

				var linkService = scope.ServiceProvider.GetRequiredService<ILinkService>();
				var logger = scope.ServiceProvider.GetRequiredService<ILogger<IJobUpdaterService>>();

				foreach (var client in _clientsAccessor.Clients)
				{
					var links = linkService.GetDoneLinksAfterID(client.LastLinkID, client.RequestID)
						.ToList()
						.Select(x => _mapper.Map<Link, RetrieveLinkOutputModel>(x))
						.ToList();

					if (links.Count > 0) {
						await client.Client.SendAsync("RetrieveUpdatedClients", links);
						client.LastLinkID = links.Last().ID;
					}
				}
			}
		}

		
	}

	
}