using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.AdminApp.Services;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;

namespace WebLuanVan.AdminApp.Controllers
{
    public class LectureController : Controller
    {
        private readonly ILectureApiClient _lectureApiClient;
        public LectureController(ILectureApiClient lectureApiClient)
        {
            _lectureApiClient = lectureApiClient;
        }
        public async Task<IActionResult> Index([FromQuery] PagingRequest request)
        {
            var session = HttpContext.Session.GetString("Token");
            if (session == null)
            {
                return RedirectToAction("Login", "User");
            }
            request.BearerToken = session;
            ViewBag.Message = TempData["result"];
            var result = await _lectureApiClient.GetLecturePaging(request);
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(LectureViewModel request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(ModelState);
            //}
            var result = await _lectureApiClient.Create(request);
            if (result)
            {
                TempData["result"] = "Thành công";
                return RedirectToAction("Index");
            }
            

            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var lecture = await _lectureApiClient.GetLectureById(id);
            return View(lecture);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(LectureViewModel request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(ModelState);
            //}
            var result = await _lectureApiClient.Update(request);
            if (result)
            {
                TempData["result"] = "Thành công";
                return RedirectToAction("Index");
            }
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var lecture = await _lectureApiClient.GetLectureById(id);
            return View(lecture);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(LectureViewModel request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(ModelState);
            //}
            var result = await _lectureApiClient.Delete(request.Id);
            if (result)
            {
                TempData["result"] = "Thành công";
                return RedirectToAction("Index");
            }
            return View(request);
        }
    }
}
