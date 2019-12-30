using System.IO;
using Microsoft.AspNetCore.Hosting;
using Monito.Web.Services.Interface;

namespace Monito.Web.Services {
	public class SpaService : ISpaService
	{
		private readonly IWebHostEnvironment _env;
		public SpaService(IWebHostEnvironment env)
		{
			_env = env;
		}

		public string GetIndexFilePath()
		{
			return Path.Combine(_env.ContentRootPath, @"wwwroot/dist/static/index.html");
		}
	}
}