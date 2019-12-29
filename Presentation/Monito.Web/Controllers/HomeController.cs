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
using Monito.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Monito.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<HomeController> _logger;
		private readonly DbContext _context;

		public HomeController(
            IWebHostEnvironment env,
            ILogger<HomeController> logger,
            DbContext context
        )
        {
            _env = env;
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var indexPath = Path.Combine(_env.ContentRootPath, @"wwwroot/dist/static/index.html");

            var user = new User{
                Email = "ciccio@example.com"
            };
            _context.Add(user);
            _context.SaveChanges();

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
