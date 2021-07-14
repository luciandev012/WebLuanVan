using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;

namespace WebLuanVan.Data.Services.Admin
{
    interface IManageFacultyServices
    {
        Task<int> Create(FacultyViewModel request);
        Task<PagedResult<FacultyViewModel>> GetFacultyPaging(PagingRequest request);
        Task<bool> Update(FacultyViewModel request);
        Task<bool> Delete(string id);
        Task<FacultyViewModel> GetFacultyById(string id);
        Task<List<FacultyViewModel>> GetFaculty();
    }
}
