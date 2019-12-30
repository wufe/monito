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
using Monito.Domain.Service.Interface;
using Monito.Web.Services;
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
    }
}
