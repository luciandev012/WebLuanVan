using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;

namespace WebLuanVan.AdminApp.Services
{
    public interface IMajorApiClient
    {
        Task<PagedResult<MajorViewModel>> GetMajorPaging(PagingRequest request);
        Task<bool> Create(MajorViewModel request);
        Task<MajorViewModel> GetMajorById(string id);
        Task<bool> Update(MajorViewModel request);
        Task<bool> Delete(string id);
    }
}
