using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.Data.Services.System.Users;
using WebLuanVan.Data.ViewModels.Request.Users;

namespace WebLuanVan.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private IMongoDatabase database;
        private readonly IUserService _userServices;
        public UsersController(IMongoClient client, IConfiguration config)
        {
            database = GetDatabase.Get(client);
            _userServices = new UserServices(database, config);
        }
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultToken = await _userServices.Authenticate(request);
            if (string.IsNullOrEmpty(resultToken))
            {
                return BadRequest("Username or Password is incorrect!");
            }
            //else
            //{
            //    HttpContext.Session.SetString("Token", resultToken);
            //}
            return Ok(resultToken);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userServices.Register(request);
            if (!result)
            {
                return BadRequest("Register is unsuccesful!");
            }
            return Ok();
        }
         [HttpGet("paging")]
        public async Task<IActionResult> GetAllUserPaging([FromQuery] GetUserPagingRequest request)
        {
            var users = await _userServices.GetUsersPaging(request);
            return Ok(users);
        }
    }
}
