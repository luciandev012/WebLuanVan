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
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var resultToken = await _userServices.Authenticate(request);
            if (resultToken == null)
            {
                return BadRequest("Username or Password is incorrect!");
            }
            //else
            //{
            //    HttpContext.Session.SetString("Token", resultToken);
            //}
            if (!resultToken.IsSuccessed)
            {
                return BadRequest(resultToken.Message);
            }
            return Ok(resultToken.ResultObj);
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
            if (!result.IsSuccessed)
            {
                return BadRequest("Register is unsuccessful!");
            }
            return Ok("Register successful, please wait to activate your account!");
        }
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllUserPaging([FromQuery] GetUserPagingRequest request)
        {
            var users = await _userServices.GetUsersPaging(request);
            return Ok(users.ResultObj);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody]UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _userServices.Update(id, request);
            if (!result.IsSuccessed)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByUsername(string id)
        {
            var result = await _userServices.GetUserById(id);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _userServices.Delete(id);
            if (!result)
            {
                return BadRequest("Cannot delete user!"); 
            }
            return Ok();
        }
    }
}
