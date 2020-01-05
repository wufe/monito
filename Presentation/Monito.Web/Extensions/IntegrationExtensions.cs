using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
				.AddSingleton<IUpdatingClientsAccessor, UpdatingClientsAccessor>();
		}
	}
}