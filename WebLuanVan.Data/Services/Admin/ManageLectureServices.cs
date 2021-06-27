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
    public class ManageLectureServices : IManageLectureServices
    {
        private readonly IMongoCollection<Lecture> _lectureCollection;
        public ManageLectureServices(IMongoDatabase database)
        {
            _lectureCollection = database.GetCollection<Lecture>("lecture");
        }
        
        public async Task<int> Create(LectureViewModel request)
        {
            
            var lecture = new Lecture()
            {
                Email = request.Email,
                LectureId = request.LectureId,
                LectureName = request.LectureName,
                LectureRole = request.LectureRole
            };
            await _lectureCollection.InsertOneAsync(lecture);
            return 1;
        }

        public async Task<bool> Delete(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var result = await _lectureCollection.DeleteOneAsync(x => x.Id == objId);
            return result.DeletedCount > 0;
        }

        public async Task<LectureViewModel> GetLectureById(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var lecture = await _lectureCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            var result = new LectureViewModel()
            {
                Id = lecture.Id.ToString(),
                Email = lecture.Email,
                LectureId = lecture.LectureId,
                LectureName = lecture.LectureName,
                LectureRole = lecture.LectureRole
            };
            return result;
        }

        public async Task<PagedResult<LectureViewModel>> GetLecturePaging(PagingRequest request)
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
            int totalRow = (int)await _lectureCollection.Find(x => x.LectureName.ToLower().Contains(request.Keyword)).CountDocumentsAsync();
            var result = await _lectureCollection.Find(x => x.LectureName.ToLower().Contains(request.Keyword))
                    .Skip((request.PageIndex - 1) * request.PageSize).Limit(request.PageSize).ToListAsync();

            List<LectureViewModel> list = new List<LectureViewModel>();
            foreach(var item in result)
            {
                LectureViewModel lvm = new LectureViewModel()
                {
                    Id = item.Id.ToString(),
                    Email = item.Email,
                    LectureId = item.LectureId,
                    LectureName = item.LectureName,
                    LectureRole = item.LectureRole
                };
                list.Add(lvm);
            }

            var pagedResult = new PagedResult<LectureViewModel>()
            {
                Items = list,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return pagedResult;
        }

        public async Task<bool> Update(LectureViewModel request)
        {
            ObjectId objId = ObjectId.Parse(request.Id);
            var lecture = await _lectureCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            if(lecture == null)
            {
                return false;
            }
            var filter = Builders<Lecture>.Filter.Eq("_id", objId);
            lecture.LectureId = request.LectureId;
            lecture.LectureName = request.LectureName;
            lecture.LectureRole = request.LectureRole;
            lecture.Email = request.Email;
            var result = await _lectureCollection.ReplaceOneAsync(filter, lecture);
            return result.MatchedCount > 0;
        }
    }
}
