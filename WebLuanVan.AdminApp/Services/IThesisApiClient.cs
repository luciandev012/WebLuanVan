using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;
using WebLuanVan.Data.ViewModels.Request.Thesis;

namespace WebLuanVan.AdminApp.Services
{
    public interface IThesisApiClient
    {
        Task<PagedResult<ThesisViewModel>> GetThesisPaging(ThesisPagingRequest request);
        Task<bool> Create(ThesisRequest request);
        List<Language> GetLanguages();
        Task<bool> Update(ThesisRequest request);
        Task<ThesisViewModel> GetThesisById(string id);
        Task<bool> Delete(string id);
        Task<bool> Status(string id);
    }
}
