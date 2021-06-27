using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;

namespace WebLuanVan.AdminApp.Services
{
    public interface ILectureApiClient
    {
        Task<PagedResult<LectureViewModel>> GetLecturePaging(PagingRequest request);
        Task<bool> Create(LectureViewModel request);
        Task<LectureViewModel> GetLectureById(string id);
        Task<bool> Update(LectureViewModel request);
        Task<bool> Delete(string id);
    }
}
