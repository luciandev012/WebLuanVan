using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;

namespace WebLuanVan.Data.Services.Admin
{
   public interface IManageStudentServices
   {
        Task<int> Create(StudentViewModel request);
        Task<PagedResult<StudentViewModel>> GetStudentPaging(PagingRequest request);
        Task<bool> Update(StudentViewModel request);
        Task<bool> Delete(string id);
        Task<StudentViewModel> GetStudentById(string id);
        Task<List<StudentViewModel>> GetStudent();
    }
}
