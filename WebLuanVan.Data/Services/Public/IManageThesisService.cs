using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.ViewModels.Request.Thesis;

namespace WebLuanVan.Data.Services.Public
{
    public interface IManageThesisService
    {
        Task<int> Create(ThesisRequest request);
        Task<int> Update(ThesisRequest request);
        Task<int> Delete(string thesisId);
        Task<List<Thesis>> Get();
        Task<Thesis> GetThesisById(string thesisId);
    }
}
