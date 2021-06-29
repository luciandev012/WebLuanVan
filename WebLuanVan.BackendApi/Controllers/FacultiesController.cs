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
    public class FacultiesController : ControllerBase
    {
        private IMongoDatabase database;
        private readonly ManageFacultyServices _manageFacultyServices;
        public FacultiesController(IMongoClient client)
        {
            database = GetDatabase.Get(client);
            _manageFacultyServices = new ManageFacultyServices(database);
        }
        [HttpPost()]

        public async Task<IActionResult> Post([FromBody] FacultyViewModel request)
        {
            var result = await _manageFacultyServices.Create(request);
            if (result == 0)
            {
                return BadRequest("Faculty not created");
            }
            //var value = await _manageThesisServices.GetThesisById(thesis.Id);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetLecturePaging([FromQuery] PagingRequest request)
        {
            var result = await _manageFacultyServices.GetFacultyPaging(request);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var lecture = await _manageFacultyServices.GetFacultyById(id);
            if (lecture == null)
            {
                return BadRequest("Cannot find Faculty!");
            }
            return Ok(lecture);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] FacultyViewModel request)
        {
            var result = await _manageFacultyServices.Update(request);
            if (!result)
            {
                return BadRequest("Faculty not updated!");
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _manageFacultyServices.Delete(id);
            if (!result)
            {
                return BadRequest("Faculty not deleted!");
            }
            return Ok();
        }
    }
}
