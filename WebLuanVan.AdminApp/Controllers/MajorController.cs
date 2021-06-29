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
    public class MajorController : Controller
    {
        private readonly IMajorApiClient _majorApiClient;
        public MajorController(IMajorApiClient facultyApiClient)
        {
            _majorApiClient = facultyApiClient;
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
            ViewBag.Keyword = request.Keyword;
            var result = await _majorApiClient.GetMajorPaging(request);
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MajorViewModel request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(ModelState);
            //}
            var result = await _majorApiClient.Create(request);
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
            var lecture = await _majorApiClient.GetMajorById(id);
            return View(lecture);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MajorViewModel request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(ModelState);
            //}
            var result = await _majorApiClient.Update(request);
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
            var lecture = await _majorApiClient.GetMajorById(id);
            return View(lecture);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(MajorViewModel request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(ModelState);
            //}
            var result = await _majorApiClient.Delete(request.Id);
            if (result)
            {
                TempData["result"] = "Thành công";
                return RedirectToAction("Index");
            }
            return View(request);
        }
    }
}
