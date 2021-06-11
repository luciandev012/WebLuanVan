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
        Task<string> Authenticate(LoginRequest request);
        Task<bool> Register(RegisterRequest request);
        Task<PagedResult<User>> GetUsersPaging(GetUserPagingRequest request);
    }
}
