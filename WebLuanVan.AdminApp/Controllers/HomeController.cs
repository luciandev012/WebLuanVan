using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.AdminApp.Models;
using WebLuanVan.AdminApp.Services;

namespace WebLuanVan.AdminApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IThesisApiClient _thesisApiClient;
        private readonly IConfiguration _configuration;

        public HomeController(IThesisApiClient thesisApiClient, IConfiguration configuration)
        {
            _thesisApiClient = thesisApiClient;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var session = HttpContext.Session.GetString("Token");
            if (session == null)
            {
                return RedirectToAction("Login", "User");
            }
            var result = await _thesisApiClient.GetCharts();
            return View(result);
        }

        
    }
}
