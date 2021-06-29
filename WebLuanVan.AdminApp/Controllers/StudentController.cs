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
    public class StudentController : Controller
    {
        private readonly IStudentApiClient _studentApiClient;
        public StudentController(IStudentApiClient studentApiClient)
        {
            _studentApiClient = studentApiClient;
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
            var result = await _studentApiClient.GetStudentPaging(request);
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(StudentViewModel request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(ModelState);
            //}
            var result = await _studentApiClient.Create(request);
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
            var lecture = await _studentApiClient.GetStudentById(id);
            return View(lecture);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(StudentViewModel request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(ModelState);
            //}
            var result = await _studentApiClient.Update(request);
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
            var lecture = await _studentApiClient.GetStudentById(id);
            return View(lecture);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(FacultyViewModel request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(ModelState);
            //}
            var result = await _studentApiClient.Delete(request.Id);
            if (result)
            {
                TempData["result"] = "Thành công";
                return RedirectToAction("Index");
            }
            return View(request);
        }
    }
}
