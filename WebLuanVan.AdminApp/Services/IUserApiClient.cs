using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
