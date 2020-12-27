using Microsoft.AspNetCore.Mvc;
using Monito.Web.Services.Interface;

namespace Monito.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpaService _spaService;

		public HomeController(ISpaService spaService)
        {
            _spaService = spaService;
        }

        public IActionResult Index()
        {
            return PhysicalFile(_spaService.GetIndexFilePath(), "text/html");
        }

        public IActionResult Version()
        {
            return new JsonResult(new { version = "2" });
        }
    }
}
