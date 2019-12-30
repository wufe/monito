using Microsoft.AspNetCore.Mvc;
using Monito.Web.Services.Interface;

namespace Monito.Web.Controllers {

	[Route("[controller]")]
	public class JobController : Controller {

		private readonly ISpaService _spaService;

		public JobController(ISpaService spaService)
        {
            _spaService = spaService;
        }

		[Route("{userUUID:guid}/{requestUUID:guid}")]
		public IActionResult Index() {
			return PhysicalFile(_spaService.GetIndexFilePath(), "text/html");
		}
	}
}