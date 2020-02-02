using Microsoft.AspNetCore.Mvc;
using Monito.Web.Services.Interface;

namespace Monito.Web.Controllers {
    public class TosController : Controller {
        private readonly ISpaService _spaService;

        public TosController(ISpaService spaService) {
            _spaService = spaService;
        }

        public IActionResult Index() {
            return PhysicalFile(_spaService.GetIndexFilePath(), "text/html");
        }
    }
}