using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Monito.Application.Model;
using Monito.ValueObjects.Output;
using Monito.Web.Services;
using Monito.Web.Services.Interface;

namespace Monito.Web.Extensions {
	public static class IntegrationExtensions {

		public static void AddHttpUtils(this IServiceCollection services) {
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		}

		public static void AddPresentationServices(this IServiceCollection services) {
			services
                    .AddScoped<IJobService, JobService>();
			services
				.AddScoped<IHttpRequestService, HttpRequestService>();
			services
				.AddScoped<ISpaService, SpaService>();
			services
				.AddScoped<IJobUpdaterService, JobUpdaterService>();
			services
				.AddScoped<IPerformanceService, PerformanceService>();

			services
				.AddSingleton<IUpdatingClientsAccessor, UpdatingClientsAccessor>();
		}
	}

	internal class MinimalLinkApplicationModelCSVMap : ClassMap<MinimalLinkApplicationModel> {
		public MinimalLinkApplicationModelCSVMap() {
			Map(m => m.ID).Name("link id");
			Map(m => m.URL).Name("url");
			Map(m => m.Output).Name("output");
			Map(m => m.StatusCode).Name("status code");
			Map(m => m.RedirectsFromLinkId).Name("redirects from link id");
			Map(m => m.RedirectsToLinkId).Name("redirects to link id");
		}
	}
}