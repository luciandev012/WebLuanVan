using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;

namespace WebLuanVan.Data.Services.Admin
{
    public interface IManageMajorServices
    {
        Task<int> Create(MajorViewModel request);
        Task<PagedResult<MajorViewModel>> GetMajorPaging(PagingRequest request);
        Task<bool> Update(MajorViewModel request);
        Task<bool> Delete(string id);
        Task<MajorViewModel> GetMajorById(string id);
        Task<List<MajorViewModel>> GetMajor();
    }
}
