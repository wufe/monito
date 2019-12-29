using Microsoft.AspNetCore.Http;

namespace Monito.Web.Services {
	public class RequestService {
		private readonly HttpContext _httpContext;

		public RequestService(IHttpContextAccessor contextAccessor)
		{
			_httpContext = contextAccessor.HttpContext;
		}

		public string GetIP() {
			return _httpContext.Connection.RemoteIpAddress.ToString();
		}
	}
}