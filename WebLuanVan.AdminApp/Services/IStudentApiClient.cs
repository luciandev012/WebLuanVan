using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;

namespace WebLuanVan.AdminApp.Services
{
    public interface IStudentApiClient
    {
        Task<PagedResult<StudentViewModel>> GetStudentPaging(PagingRequest request);
        Task<bool> Create(StudentViewModel request);
        Task<StudentViewModel> GetStudentById(string id);
        Task<bool> Update(StudentViewModel request);
        Task<bool> Delete(string id);
        Task<List<MajorViewModel>> GetMajor();
        Task<List<FacultyViewModel>> GetFaculty();
    }
}
