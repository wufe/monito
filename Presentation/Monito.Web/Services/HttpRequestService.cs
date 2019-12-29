using Microsoft.AspNetCore.Http;
using Monito.Web.Services.Interface;

namespace Monito.Web.Services {
	public class HttpRequestService : IHttpRequestService {
		private readonly HttpContext _httpContext;

		public HttpRequestService(IHttpContextAccessor contextAccessor)
		{
			_httpContext = contextAccessor.HttpContext;
		}

		public string GetIP() {
			return _httpContext.Connection.RemoteIpAddress.ToString();
		}
	}
}