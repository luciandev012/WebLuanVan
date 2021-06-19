using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request.Users;

namespace WebLuanVan.AdminApp.Services
{
    public class UserApiClient : IUserApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ApiResult<string>> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var response = await client.PostAsync("/api/users/authenticate", httpContent);
            var token = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                
                return new ApiSuccessResult<string>(token);
            }
            return new ApiErrorResult<string>(token);
        }

        public async Task<bool> Delete(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var response = await client.DeleteAsync($"/api/users/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }

        public async Task<ApiResult<User>> GetUserById(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/users/{id}");
            var body = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<User>>(body);
            }
            //var users = JsonConvert.DeserializeObject<PagedResult<User>>(body);
            return JsonConvert.DeserializeObject<ApiErrorResult<User>>(body);
        }

        public async Task<PagedResult<User>> GetUsersPaging(GetUserPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", request.BearerToken);
            var response = await client.GetAsync($"/api/users/paging?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}");
            var body = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<PagedResult<User>>(body);
            return users;
        }

        public async Task<HttpResponseMessage> Register(RegisterRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5001");
            var response = await client.PostAsync("/api/users/register", httpContent);
            //var result = await response.Content.ReadAsStringAsync();
            //response.
            return response;
        }

        public async Task<bool> Status(string id)
        {
            var json = JsonConvert.SerializeObject("");
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var response = await client.PutAsync($"/api/users/{id}/status", httpContent);
           //ar result = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode;
        }

        public async Task<ApiResult<bool>> Update(string id, UserUpdateRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var response = await client.PutAsync($"/api/users/{id}", httpContent);
            var result = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<ApiSuccessResult<bool>>(result);
            }
            return JsonConvert.DeserializeObject<ApiErrorResult<bool>>(result);
        }
    }
}
