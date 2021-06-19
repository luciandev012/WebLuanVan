using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.AdminApp.Services;
using WebLuanVan.Data.ViewModels.Request.Thesis;

namespace WebLuanVan.AdminApp.Controllers
{
    public class ThesisController : Controller
    {
        private readonly IThesisApiClient _thesisApiClient;
        private readonly IConfiguration _configuration;
        public ThesisController(IThesisApiClient thesisApiClient, IConfiguration configuration)
        {
            _thesisApiClient = thesisApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword, string field, int pageIndex = 1, int pageSize = 10)
        {
            var session = HttpContext.Session.GetString("Token");
            if (session == null)
            {
                return RedirectToAction("Login", "User");
            }
            var request = new ThesisPagingRequest()
            {
                BearerToken = session,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Field = field
            };
            var data = await _thesisApiClient.GetThesisPaging(request);
            ViewBag.Keyword = keyword;
            ViewBag.Message = TempData["result"];
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ThesisRequest request)
        {
            var result = await _thesisApiClient.Create(request);
            if (result)
            {
                TempData["result"] = "Thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Thêm không thành công!");
            
            return View(request);
        }
    }
}
