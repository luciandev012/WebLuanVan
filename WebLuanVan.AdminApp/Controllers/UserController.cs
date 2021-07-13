using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.AdminApp.Models;
using WebLuanVan.AdminApp.Services;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request.Users;

namespace WebLuanVan.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        public UserController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
        }
        public async Task<IActionResult> Index(string keyword, int pageIndex = 1, int pageSize = 10)
        {
            var session = HttpContext.Session.GetString("Token");
            if(session == null)
            {
                return RedirectToAction("Login", "User");
            }
            var request = new GetUserPagingRequest()
            {
                BearerToken = session,
                Keyword = keyword,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetUsersPaging(request);
            ViewBag.Keyword = keyword;
            ViewBag.Message = TempData["result"];
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }
            var token = await _userApiClient.Authenticate(request);
            if (!token.IsSuccessed)
            {
                ModelState.AddModelError("", token.Message);
                return View();
            }
            var userPrincipal = ValidationToken(token.ResultObj);
            var authProperties = new AuthenticationProperties()
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                IsPersistent = true
            };
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProperties);
            var userRole = userPrincipal.Claims.ToArray()[1].Value.ToString();
            
            if (userRole == "User")
            {
                HttpContext.Session.SetString("Token", token.ResultObj);
                return RedirectToAction("Index", "NormalUser");
            }
            HttpContext.Session.SetString("Token", token.ResultObj);
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "User");
        }
        private ClaimsPrincipal ValidationToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;
            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();
            validationParameters.ValidateLifetime = true;
            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);
            return principal;
        }
        [HttpGet]
        public async Task<IActionResult> Forbidden()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(ModelState);
            //}
            var result = await _userApiClient.Register(request);
            if (result.IsSuccessStatusCode)
            {
                TempData["result"] = "Thành công";
                return RedirectToAction("Index");
            }
            var message = await result.Content.ReadAsStringAsync();
            
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var result = await _userApiClient.GetUserById(id);
            return View(result.ResultObj);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(User user)
        {
            UserUpdateRequest request = new UserUpdateRequest()
            {
                LastName = user.LastName,
                FirstName = user.FirstName
            };
            var result = await _userApiClient.Update(user.Id, request);
            if (result.IsSuccessed)
            {
                TempData["result"] = "Thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }
        //[HttpDelete]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userApiClient.GetUserById(id);
            return View(user.ResultObj);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(User user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _userApiClient.Delete(user.Id);
            if (result)
            {
                TempData["result"] = "Thành công";
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Cannot delete user!");
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> Status(string id)
        {
            await _userApiClient.Status(id);
            return RedirectToAction("Index");
        }
    }
}
