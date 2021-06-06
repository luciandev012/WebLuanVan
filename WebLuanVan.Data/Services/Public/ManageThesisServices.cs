using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;

namespace WebLuanVan.Data.Services.Public
{
    public class ManageThesisServices : IManageThesisService
    {
        private readonly IMongoCollection<Thesis> _thesisCollection;
        public ManageThesisServices(IMongoDatabase database)
        {
            _thesisCollection = database.GetCollection<Thesis>("thesis");
        }
        public async Task<int> Create(Thesis thesis)
        {
            thesis.CreatedAt = DateTime.Now;
            await _thesisCollection.InsertOneAsync(thesis);
            return 1;
        }

        public Task<int> Delete(string thesisId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Thesis>> Get()
        {
            var thesis = await _thesisCollection.FindSync(_ => true).ToListAsync();
            return thesis;
        }

        public Task<Thesis> GetThesisById(string thesisId)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(Thesis thesis)
        {
            throw new NotImplementedException();
        }
    }
}
