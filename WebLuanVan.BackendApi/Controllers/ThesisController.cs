using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.Data.Services.Public;

namespace WebLuanVan.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThesisController : ControllerBase
    {
        private IMongoDatabase database;
        private readonly IManageThesisService _manageThesisServices;
        public ThesisController(IMongoClient client)
        {
            database = GetDatabase.Get(client);
            _manageThesisServices = new ManageThesisServices(database);
        }
        
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var thesis = await _manageThesisServices.Get();
            return Ok(thesis);
        }
    }
}
