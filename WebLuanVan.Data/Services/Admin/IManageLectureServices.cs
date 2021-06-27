using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;

namespace WebLuanVan.Data.Services.Admin
{
    public interface IManageLectureServices
    {
        Task<int> Create(LectureViewModel request);
        Task<PagedResult<LectureViewModel>> GetLecturePaging(PagingRequest request);
        Task<bool> Update(LectureViewModel request);
        Task<bool> Delete(string id);
        Task<LectureViewModel> GetLectureById(string id);
       
    }
}
