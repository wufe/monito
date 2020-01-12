using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Monito.Web.Extensions;
using Monito.Web.Services.Interface;

namespace Monito.Web.Services {
	public class HttpRequestService : IHttpRequestService {
		private readonly HttpContext _httpContext;
		private readonly ILogger<IHttpRequestService> _logger;

		public HttpRequestService(
			IHttpContextAccessor contextAccessor,
			ILogger<IHttpRequestService> logger
		)
		{
			_httpContext = contextAccessor.HttpContext;
			_logger = logger;
		}

		public string GetIP() {
			var ip = _httpContext.GetRemoteIPAddress().ToString();
			_logger.LogInformation(ip);
			return ip;
		}
	}
}