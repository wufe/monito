using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monito.Web.Models;
using Monito.Database.Interface;
using Monito.Database.Entities;

namespace Monito.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IWebHostEnvironment env,
            ILogger<HomeController> logger
        )
        {
            _env = env;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var indexPath = Path.Combine(_env.ContentRootPath, @"wwwroot/dist/static/index.html");
            return PhysicalFile(indexPath, "text/html");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
