using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.Services.Common;
using WebLuanVan.Data.Services.Public;
using WebLuanVan.Data.ViewModels.Request.Thesis;

namespace WebLuanVan.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThesisController : ControllerBase
    {
        private IMongoDatabase database;
        private readonly IManageThesisService _manageThesisServices;
        public ThesisController(IMongoClient client, IStorageService storageService)
        {
            database = GetDatabase.Get(client);
            _manageThesisServices = new ManageThesisServices(database, storageService);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var thesis = await _manageThesisServices.Get();
            return Ok(thesis);
        }
        [HttpGet("/{thesisId}")]
        public async Task<IActionResult> Get(string thesisId)
        {
            var thesis = await _manageThesisServices.GetThesisById(thesisId);
            return Ok(thesis);
        }
        [HttpPost()]
        public async Task<IActionResult> Post([FromForm]ThesisRequest thesis)
        {
            var result = await _manageThesisServices.Create(thesis);
            if(result == 0)
            {
                return BadRequest("Thesis not created");
            }
            var value = await _manageThesisServices.GetThesisById(thesis.ThesisId);
            return Ok(value);
        }
        [HttpPut()]
        public async Task<IActionResult> Put([FromForm] ThesisRequest thesis)
        {
            var result = await _manageThesisServices.Update(thesis);
            if(result == 0)
            {
                return BadRequest("Thesis not updated");
            }
            var value = await _manageThesisServices.GetThesisById(thesis.ThesisId);
            return Ok(value);
        }
        [HttpDelete("/{thesisId}")]
        public async Task<IActionResult> Delete(string thesisId)
        {
            var result = await _manageThesisServices.Delete(thesisId);
            if (result == 0)
            {
                return BadRequest("Thesis not deleted");
            }
            
            return Ok();
        }
    }
}
