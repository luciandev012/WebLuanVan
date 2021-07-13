using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.AdminApp.Services;
using WebLuanVan.Data.ViewModels.ModelBinding;
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
        public async Task<IActionResult> Index([FromQuery] ThesisPagingRequest request)
        {
            var session = HttpContext.Session.GetString("Token");
            if (session == null)
            {
                return RedirectToAction("Login", "User");
            }
            //var request = new ThesisPagingRequest()
            //{
            //    BearerToken = session,
            //    Keyword = keyword,
            //    PageIndex = pageIndex,
            //    PageSize = pageSize,

            //};
            request.BearerToken = session;
            request.Role = "Admin";
            var data = await _thesisApiClient.GetThesisPaging(request);
            ViewBag.Keyword = request.Keyword;
            var languages = _thesisApiClient.GetLanguages();
            ViewBag.Languages = languages.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.LanguageId,
                Selected = (!string.IsNullOrEmpty(request.LanguageId)) && request.LanguageId == x.LanguageId
            });
            ViewBag.Message = TempData["result"];
            ViewBag.FilePath = "https://localhost:5001/user-content/";
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
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
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var item = await _thesisApiClient.GetThesisById(id);
            string debateLecture = "";
            foreach(var i in item.DebateLectureId)
            {
                debateLecture += i.ToString();
            }
            
            var request = new ThesisRequest()
            {
                ThesisName = item.ThesisName,
                ThesisId = item.ThesisId,
                Id = item.Id.ToString(),
                AcademicYear = item.AcademicYear,
                DebateLectureId = debateLecture,
                FacultyId = item.FacultyId,
                FinishedAt = item.FinishedAt,
                GuideLectureId = item.GuideLectureId,
                IsProtected = item.IsProtected,
                Language = item.Language,
                //FileContent = item.Content,
                MakedAt = item.MakedAt,
                Phase = item.Phase,
                Score = (int)item.Score,
                ProtectedAt = item.ProtectedAt,
                StudentId = item.StudentId,
                Year = item.Year
            };
            ViewBag.ContentString = item.Content;
            return View(request);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] ThesisRequest request, string id)
        {
            request.Id = id;
            var result = await _thesisApiClient.Update(request);
            if (result)
            {
                TempData["result"] = "Thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Sửa không thành công!");

            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var thesis = await _thesisApiClient.GetThesisById(id);
            return View(thesis);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(ThesisViewModel thesis)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _thesisApiClient.Delete(thesis.Id);
            if (result)
            {
                TempData["result"] = "Thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Cannot delete user!");
            return View(thesis);
        }
        [HttpGet]
        public async Task<IActionResult> Status(string id)
        {
            await _thesisApiClient.Status(id);
            return RedirectToAction("Index");
        }
    }
}
