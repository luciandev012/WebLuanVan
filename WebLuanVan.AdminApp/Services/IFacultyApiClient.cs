using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;

namespace WebLuanVan.AdminApp.Services
{
    public interface IFacultyApiClient
    {
        Task<PagedResult<FacultyViewModel>> GetFacultyPaging(PagingRequest request);
        Task<bool> Create(FacultyViewModel request);
        Task<FacultyViewModel> GetFacultyById(string id);
        Task<bool> Update(FacultyViewModel request);
        Task<bool> Delete(string id);
        
    }
}
