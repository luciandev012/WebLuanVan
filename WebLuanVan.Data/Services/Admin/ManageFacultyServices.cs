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
    public class ManageFacultyServices : IManageFacultyServices
    {
        private readonly IMongoCollection<Faculty> _facultyCollection;
        public ManageFacultyServices(IMongoDatabase database)
        {
            _facultyCollection = database.GetCollection<Faculty>("faculty");
        }

        public async Task<int> Create(FacultyViewModel request)
        {

            var faculty = new Faculty()
            {
               FacultyId = request.FacultyId,
               FacultyName = request.FacultyName,
               
            };
            await _facultyCollection.InsertOneAsync(faculty);
            return 1;
        }

        public async Task<bool> Delete(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var result = await _facultyCollection.DeleteOneAsync(x => x.Id == objId);
            return result.DeletedCount > 0;
        }

        public async Task<List<FacultyViewModel>> GetFaculty()
        {
            var res = await _facultyCollection.Find(x => true).ToListAsync();
            List<FacultyViewModel> fvm = new List<FacultyViewModel>();
            foreach(var f in res)
            {
                FacultyViewModel fv = new FacultyViewModel()
                {
                    FacultyId = f.FacultyId,
                    FacultyName = f.FacultyName
                };
                fvm.Add(fv);
            }
            return fvm;
        }

        public async Task<FacultyViewModel> GetFacultyById(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var faculty = await _facultyCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            var result = new FacultyViewModel()
            {
                Id = faculty.Id.ToString(),
                FacultyName = faculty.FacultyName,
                FacultyId = faculty.FacultyId
            };
            return result;
        }

        public async Task<PagedResult<FacultyViewModel>> GetFacultyPaging(PagingRequest request)
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
            int totalRow = (int)await _facultyCollection.Find(x => x.FacultyName.ToLower().Contains(request.Keyword)).CountDocumentsAsync();
            var result = await _facultyCollection.Find(x => x.FacultyName.ToLower().Contains(request.Keyword))
                    .Skip((request.PageIndex - 1) * request.PageSize).Limit(request.PageSize).ToListAsync();

            List<FacultyViewModel> list = new List<FacultyViewModel>();
            foreach (var item in result)
            {
                FacultyViewModel fvm = new FacultyViewModel()
                {
                    Id = item.Id.ToString(),
                    FacultyName = item.FacultyName,
                    FacultyId = item.FacultyId
                };
                list.Add(fvm);
            }

            var pagedResult = new PagedResult<FacultyViewModel>()
            {
                Items = list,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return pagedResult;
        }

        public async Task<bool> Update(FacultyViewModel request)
        {
            ObjectId objId = ObjectId.Parse(request.Id);
            var faculty = await _facultyCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            if (faculty == null)
            {
                return false;
            }
            var filter = Builders<Faculty>.Filter.Eq("_id", objId);
            faculty.FacultyId = request.FacultyId;
            faculty.FacultyName = request.FacultyName;
            var result = await _facultyCollection.ReplaceOneAsync(filter, faculty);
            return result.MatchedCount > 0;
        }
    }
}
