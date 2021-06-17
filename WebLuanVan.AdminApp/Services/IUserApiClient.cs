using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request.Users;

namespace WebLuanVan.AdminApp.Services
{
    public interface IUserApiClient
    {
        Task<string> Authenticate(LoginRequest request);
        Task<PagedResult<User>> GetUsersPaging(GetUserPagingRequest request);
        Task<HttpResponseMessage> Register(RegisterRequest request);
        Task<ApiResult<bool>> Update(string id, UserUpdateRequest request);
        Task<ApiResult<User>> GetUserById(string id);
        Task<bool> Delete(string id);
    }
}
