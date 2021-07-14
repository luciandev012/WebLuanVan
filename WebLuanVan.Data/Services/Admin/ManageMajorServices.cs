using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;

namespace WebLuanVan.Data.Services.Admin
{
    public class ManageMajorServices : IManageMajorServices
    {
        private readonly IMongoCollection<Major> _majorCollection;
        public ManageMajorServices(IMongoDatabase database)
        {
            _majorCollection = database.GetCollection<Major>("major");
        }

        public async Task<int> Create(MajorViewModel request)
        {

            var major = new Major()
            {
                MajorId = request.MajorId,
                MajorName = request.MajorName,
                FacultyId = request.FacultyId
            };
            await _majorCollection.InsertOneAsync(major);
            return 1;
        }

        public async Task<bool> Delete(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var result = await _majorCollection.DeleteOneAsync(x => x.Id == objId);
            return result.DeletedCount > 0;
        }

        public async Task<List<MajorViewModel>> GetMajor()
        {
            var res = await _majorCollection.Find(x => true).ToListAsync();
            List<MajorViewModel> mvm = new List<MajorViewModel>();
            foreach (var m in res)
            {
                MajorViewModel mv = new MajorViewModel()
                {
                    MajorName = m.MajorName,
                    MajorId = m.MajorId
                };
                mvm.Add(mv);
            }
            return mvm;
        }

        public async Task<MajorViewModel> GetMajorById(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var major = await _majorCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            var result = new MajorViewModel()
            {
                Id = major.Id.ToString(),
                MajorName = major.MajorName,
                MajorId = major.MajorId,
                FacultyId = major.FacultyId
            };
            return result;
        }

        public async Task<PagedResult<MajorViewModel>> GetMajorPaging(PagingRequest request)
        {
            if (string.IsNullOrEmpty(request.Keyword))
            {
                request.Keyword = "";
            }
            if (request.PageIndex == 0)
            {
                request.PageIndex = 1;
            }
            if (request.PageSize == 0)
            {
                request.PageSize = 5;
            }
            int totalRow = (int)await _majorCollection.Find(x => x.MajorName.ToLower().Contains(request.Keyword)).CountDocumentsAsync();
            var result = await _majorCollection.Find(x => x.MajorName.ToLower().Contains(request.Keyword))
                    .Skip((request.PageIndex - 1) * request.PageSize).Limit(request.PageSize).ToListAsync();

            List<MajorViewModel> list = new List<MajorViewModel>();
            foreach (var item in result)
            {
                MajorViewModel mvm = new MajorViewModel()
                {
                    Id = item.Id.ToString(),
                    MajorName = item.MajorName,
                    MajorId = item.MajorId,
                    FacultyId = item.FacultyId
                };
                list.Add(mvm);
            }

            var pagedResult = new PagedResult<MajorViewModel>()
            {
                Items = list,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return pagedResult;
        }

        public async Task<bool> Update(MajorViewModel request)
        {
            ObjectId objId = ObjectId.Parse(request.Id);
            var major = await _majorCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            if (major == null)
            {
                return false;
            }
            var filter = Builders<Major>.Filter.Eq("_id", objId);
            major.FacultyId = request.FacultyId;
            major.MajorId = request.MajorId;
            major.MajorName = request.MajorName;
            var result = await _majorCollection.ReplaceOneAsync(filter, major);
            return result.MatchedCount > 0;
        }

        
    }

}
