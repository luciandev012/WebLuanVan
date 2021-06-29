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
    public class ManageStudentServices : IManageStudentServices
    {
        private readonly IMongoCollection<Student> _studentCollection;
        public ManageStudentServices(IMongoDatabase database)
        {
            _studentCollection = database.GetCollection<Student>("student");
        }

        public async Task<int> Create(StudentViewModel request)
        {

            var student = new Student()
            {
                FacultyId = request.FacultyId,
                StudentId = request.StudentId,
                MajorId = request.MajorId,
                PhoneNumber = request.PhoneNumber,
                StudentName = request.StudentName

            };
            await _studentCollection.InsertOneAsync(student);
            return 1;
        }

        public async Task<bool> Delete(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var result = await _studentCollection.DeleteOneAsync(x => x.Id == objId);
            return result.DeletedCount > 0;
        }

        public async Task<StudentViewModel> GetStudentById(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var student = await _studentCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            var result = new StudentViewModel()
            {
                Id = student.Id.ToString(),
                StudentName = student.StudentName,
                FacultyId = student.FacultyId,
                PhoneNumber = student.PhoneNumber,
                MajorId = student.MajorId,
                StudentId = student.StudentId
            };
            return result;
        }

        public async Task<PagedResult<StudentViewModel>> GetStudentPaging(PagingRequest request)
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
            int totalRow = (int)await _studentCollection.Find(x => x.StudentName.ToLower().Contains(request.Keyword)).CountDocumentsAsync();
            var result = await _studentCollection.Find(x => x.StudentName.ToLower().Contains(request.Keyword))
                    .Skip((request.PageIndex - 1) * request.PageSize).Limit(request.PageSize).ToListAsync();

            List<StudentViewModel> list = new List<StudentViewModel>();
            foreach (var item in result)
            {
                StudentViewModel svm = new StudentViewModel()
                {
                    Id = item.Id.ToString(),
                    StudentName = item.StudentName,
                    FacultyId = item.FacultyId,
                    PhoneNumber = item.PhoneNumber,
                    StudentId = item.StudentId,
                    MajorId = item.MajorId
                };
                list.Add(svm);
            }

            var pagedResult = new PagedResult<StudentViewModel>()
            {
                Items = list,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return pagedResult;
        }

        public async Task<bool> Update(StudentViewModel request)
        {
            ObjectId objId = ObjectId.Parse(request.Id);
            var student = await _studentCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            if (student == null)
            {
                return false;
            }
            var filter = Builders<Student>.Filter.Eq("_id", objId);
            student.MajorId = request.MajorId;
            student.PhoneNumber = request.PhoneNumber;
            student.StudentId = request.StudentId;
            student.StudentName = request.StudentName;
            student.FacultyId = request.FacultyId;
            var result = await _studentCollection.ReplaceOneAsync(filter, student);
            return result.MatchedCount > 0;
        }
    }
    
}
