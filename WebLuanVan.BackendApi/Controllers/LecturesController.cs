using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.Data.Services.Admin;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;

namespace WebLuanVan.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LecturesController : ControllerBase
    {
        private IMongoDatabase database;
        private readonly IManageLectureServices _manageLectureServices;
        public LecturesController(IMongoClient client)
        {
            database = GetDatabase.Get(client);
            _manageLectureServices = new ManageLectureServices(database);
        }
        [HttpPost()]
        
        public async Task<IActionResult> Post([FromBody] LectureViewModel request)
        {
            var result = await _manageLectureServices.Create(request);
            if (result == 0)
            {
                return BadRequest("Lecture not created");
            }
            //var value = await _manageThesisServices.GetThesisById(thesis.Id);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetLecturePaging([FromQuery] PagingRequest request)
        {
            var result = await _manageLectureServices.GetLecturePaging(request);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var lecture = await _manageLectureServices.GetLectureById(id);
            if(lecture == null)
            {
                return BadRequest("Cannot find lecture!");
            }
            return Ok(lecture);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] LectureViewModel request)
        {
            var result = await _manageLectureServices.Update(request);
            if (!result)
            {
                return BadRequest("Lecture not updated!");
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _manageLectureServices.Delete(id);
            if (!result)
            {
                return BadRequest("Lecture not deleted!");
            }
            return Ok();
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetLecture()
        {
            var res = await _manageLectureServices.GetLecture();
            if(res != null)
            {
                return Ok(res);
            }
            return BadRequest();
        }
    }
}
