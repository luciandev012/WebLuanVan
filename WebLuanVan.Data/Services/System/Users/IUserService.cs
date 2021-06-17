using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request.Users;

namespace WebLuanVan.Data.Services.System.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<PagedResult<User>>> GetUsersPaging(GetUserPagingRequest request);
        Task<ApiResult<bool>> Update(string username, UserUpdateRequest request);
        Task<ApiResult<User>> GetUserById(string id);
        Task<bool> Delete(string id);
    }
}
