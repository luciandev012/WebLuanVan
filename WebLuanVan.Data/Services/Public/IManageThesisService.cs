using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request.Thesis;

namespace WebLuanVan.Data.Services.Public
{
    public interface IManageThesisService
    {
        Task<int> Create(ThesisRequest request);
        Task<int> Update(ThesisRequest request);
        Task<int> Delete(string id);
        Task<ApiResult<PagedResult<ThesisViewModel>>> Get(ThesisPagingRequest request);
        Task<ThesisViewModel> GetThesisById(string id);
        Task<bool> ChangeThesisStatus(string id);
    }
}
