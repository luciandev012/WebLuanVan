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
    public class MajorsController : ControllerBase
    {
        private IMongoDatabase database;
        private readonly ManageMajorServices _manageMajorServices;
        public MajorsController(IMongoClient client)
        {
            database = GetDatabase.Get(client);
            _manageMajorServices = new ManageMajorServices(database);
        }
        [HttpPost()]

        public async Task<IActionResult> Post([FromBody] MajorViewModel request)
        {
            var result = await _manageMajorServices.Create(request);
            if (result == 0)
            {
                return BadRequest("Major not created");
            }
            //var value = await _manageThesisServices.GetThesisById(thesis.Id);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetLecturePaging([FromQuery] PagingRequest request)
        {
            var result = await _manageMajorServices.GetMajorPaging(request);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var lecture = await _manageMajorServices.GetMajorById(id);
            if (lecture == null)
            {
                return BadRequest("Cannot find Major!");
            }
            return Ok(lecture);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MajorViewModel request)
        {
            var result = await _manageMajorServices.Update(request);
            if (!result)
            {
                return BadRequest("Faculty not updated!");
            }
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _manageMajorServices.Delete(id);
            if (!result)
            {
                return BadRequest("Faculty not deleted!");
            }
            return Ok();
        }
        [HttpGet("list")]
        public async Task<IActionResult> GetListMajor()
        {
            var res = await _manageMajorServices.GetMajor();
            if (res != null)
            {
                return Ok(res);
            }
            return BadRequest();
        }
    }
}
