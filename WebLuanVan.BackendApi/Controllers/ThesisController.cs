using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.Services.Common;
using WebLuanVan.Data.Services.Public;
using WebLuanVan.Data.ViewModels.Request;
using WebLuanVan.Data.ViewModels.Request.Thesis;

namespace WebLuanVan.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ThesisController : ControllerBase
    {
        private IMongoDatabase database;
        private readonly IManageThesisService _manageThesisServices;
        
        public ThesisController(IMongoClient client, IStorageService storageService, IElasticClient elasticClient)
        {
            database = GetDatabase.Get(client);
            _manageThesisServices = new ManageThesisServices(database, storageService, elasticClient);
        }

        [HttpGet]
        public async Task<IActionResult> GetThesisPaging([FromQuery] ThesisPagingRequest request)
        {
            var thesis = await _manageThesisServices.Get(request);
            return Ok(thesis.ResultObj);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var thesis = await _manageThesisServices.GetThesisById(id);
            return Ok(thesis);
        }
        [HttpPost()]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] ThesisRequest thesis)
        {
            var result = await _manageThesisServices.Create(thesis);
            if (result == 0)
            {
                return BadRequest("Thesis not created");
            }
            //var value = await _manageThesisServices.GetThesisById(thesis.Id);
            return Ok();
        }
        [HttpPut()]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Put([FromForm] ThesisRequest thesis)
        {
            var result = await _manageThesisServices.Update(thesis);
            if (result == 0)
            {
                return BadRequest("Thesis not updated");
            }
            //var value = await _manageThesisServices.GetThesisById(thesis.ThesisId);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _manageThesisServices.Delete(id);
            if (result == 0)
            {
                return BadRequest("Thesis not deleted");
            }

            return Ok();
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> Status(string id)
        {
            var result = await _manageThesisServices.ChangeThesisStatus(id);
            if (!result)
            {
                return BadRequest("Something went wrong!");
            }
            return Ok();
        }
    }
}
