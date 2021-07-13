using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.AdminApp.Services;
using WebLuanVan.Data.ViewModels.Request.Thesis;

namespace WebLuanVan.AdminApp.Controllers
{
    public class NormalUserController : Controller
    {
        private readonly IThesisApiClient _thesisApiClient;
        private readonly IConfiguration _configuration;
        public NormalUserController(IThesisApiClient thesisApiClient, IConfiguration configuration)
        {
            _thesisApiClient = thesisApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index([FromQuery] ThesisPagingRequest request)
        {
            var session = HttpContext.Session.GetString("Token");
            if (session == null)
            {
                return RedirectToAction("Login", "User");
            }
            request.BearerToken = session;
            request.Role = "User";
            request.Major = "a";
            request.StudentCode = "a";
            request.Faculty = "a";
            request.Class = "a";
            request.AcademicYear = "1";
            //request.
            var data = await _thesisApiClient.GetThesisPaging(request);
            ViewBag.Keyword = request.Keyword;
            var languages = _thesisApiClient.GetLanguages();
            ViewBag.Languages = languages.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.LanguageId,
                Selected = (!string.IsNullOrEmpty(request.LanguageId)) && request.LanguageId == x.LanguageId
            });
            
            ViewBag.FilePath = "https://localhost:5001/user-content/";
            return View(data);
            
        }
    }
}
