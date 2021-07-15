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
    public class StudentsController : ControllerBase
    {
        private IMongoDatabase database;
        private readonly ManageStudentServices _manageStudentServices;
        public StudentsController(IMongoClient client)
        {
            database = GetDatabase.Get(client);
            _manageStudentServices = new ManageStudentServices(database);
        }
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] StudentViewModel request)
        {
            var result = await _manageStudentServices.Create(request);
            if (result == 0)
            {
                return BadRequest("Student not created");
            }
            //var value = await _manageThesisServices.GetThesisById(thesis.Id);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetLecturePaging([FromQuery] PagingRequest request)
        {
            var result = await _manageStudentServices.GetStudentPaging(request);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var student = await _manageStudentServices.GetStudentById(id);
            if (student == null)
            {
                return BadRequest("Cannot find Faculty!");
            }
            return Ok(student);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] StudentViewModel request)
        {
            var result = await _manageStudentServices.Update(request);
            if (!result)
            {
                return BadRequest("Student not updated!");
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _manageStudentServices.Delete(id);
            if (!result)
            {
                return BadRequest("Student not deleted!");
            }
            return Ok();
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetStudent()
        {
            var res = await _manageStudentServices.GetStudent();
            if(res != null)
            {
                return Ok(res);
            }
            return BadRequest();
        }
    }
}
