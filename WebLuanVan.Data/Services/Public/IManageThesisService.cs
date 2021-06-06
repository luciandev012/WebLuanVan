using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;

namespace WebLuanVan.Data.Services.Public
{
    public interface IManageThesisService
    {
        Task<int> Create(Thesis thesis);
        Task<int> Update(Thesis thesis);
        Task<int> Delete(string thesisId);
        Task<List<Thesis>> Get();
        Task<Thesis> GetThesisById(string thesisId);
    }
}
