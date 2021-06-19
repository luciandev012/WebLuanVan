using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.Services.Common;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request.Thesis;

namespace WebLuanVan.Data.Services.Public
{
    public class ManageThesisServices : IManageThesisService
    {
        private readonly IMongoCollection<Thesis> _thesisCollection;
        private readonly IStorageService _storageService;
        public ManageThesisServices(IMongoDatabase database, IStorageService storageService)
        {
            _thesisCollection = database.GetCollection<Thesis>("thesis");
            _storageService = storageService;
        }
        public async Task<int> Create(ThesisRequest request)
        {
            var thesis = new Thesis()
            {
                ThesisName = request.ThesisName,
                ThesisId = request.ThesisId,
                AcademicYear = request.AcademicYear,
                MakedAt = request.MakedAt,
                FacultyId = request.FacultyId,
                GuideLectureId = request.GuideLectureId,
                DebateLectureId = request.DebateLectureId,
                Phase = request.Phase,
                StudentId = request.StudentId,
                Year = request.Year,
                Language = request.Language,
                FinishedAt = request.FinishedAt,
                IsProtected = request.IsProtected,
                ProtectedAt = request.ProtectedAt,
                Score = request.Score
            };
            if(request.FileContent != null)
            {
                thesis.Content = await this.SaveFile(request.FileContent);
            }
            await _thesisCollection.InsertOneAsync(thesis);

            return 1;
        }

        public async Task<int> Delete(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var filter = Builders<Thesis>.Filter.Eq("thesisId", objId);
            Thesis thesis = await GetThesisById(id);
            await _storageService.DeleteFileAsynce(thesis.Content);
            await _thesisCollection.DeleteOneAsync(filter);

            return 1;
        }

        public async Task<ApiResult<PagedResult<ThesisViewModel>>> Get(ThesisPagingRequest request)
        {
            if(request.Field == null)
            {
                request.Field = "thesisName";
            }
            if (string.IsNullOrEmpty(request.Keyword))
            {
                request.Keyword = "";
            }
            //var filter = Builders<Thesis>.Filter.Eq(x => x.ThesisName.ToLower().Contains(request.Keyword));
            int totalRow = (int)await _thesisCollection.Find(x => x.ThesisName.ToLower().Contains(request.Keyword)).CountDocumentsAsync();
            var result = await _thesisCollection.Find(x => x.ThesisName.ToLower().Contains(request.Keyword))
                    .Skip((request.PageIndex - 1) * request.PageSize).Limit(request.PageSize).ToListAsync();

            List<ThesisViewModel> listThesis = new List<ThesisViewModel>();
            foreach (var item in result) //Binding thesis from collection account from database
            {
                ThesisViewModel thesis = new ThesisViewModel() 
                {
                    ThesisName = item.ThesisName,
                    ThesisId = item.ThesisId,
                    Id = item.Id.ToString(),
                    AcademicYear = item.AcademicYear,
                    DebateLectureId = item.DebateLectureId,
                    FacultyId = item.FacultyId,
                    FinishedAt = item.FinishedAt,
                    GuideLectureId = item.GuideLectureId,
                    IsProtected = item.IsProtected,
                    Language = item.Language,
                    Content = item.Content,
                    MakedAt = item.MakedAt,
                    Phase = item.Phase,
                    Score = (int)item.Score,
                    ProtectedAt = item.ProtectedAt,
                    StudentId = item.StudentId,
                    Year = item.Year
                };

                listThesis.Add(thesis);
            }

            var pagedResult = new PagedResult<ThesisViewModel>
            {
                Items = listThesis,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecord = totalRow
            };
            return new ApiSuccessResult<PagedResult<ThesisViewModel>>(pagedResult);
        }

       

        public async Task<Thesis> GetThesisById(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var thesis = await _thesisCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            return thesis;
        }

        public async Task<int> Update(ThesisRequest request)
        {
            var thesis = await GetThesisById(request.ThesisId);
            if(thesis == null)
            {
                throw new Exception($"Cannot find thesis with id{request.ThesisId}");
            }
            var filter = Builders<Thesis>.Filter.Eq("thesisId", request.ThesisId);
            
            thesis.ThesisName = request.ThesisName;
            thesis.ThesisId = request.ThesisId;
            thesis.AcademicYear = request.AcademicYear;
            thesis.FacultyId = request.FacultyId;
            thesis.GuideLectureId = request.GuideLectureId;
            thesis.Phase = request.Phase;
            thesis.StudentId = request.StudentId;
            thesis.Year = request.Year;
            thesis.Language = request.Language;
            thesis.DebateLectureId = request.DebateLectureId;
            thesis.Score = request.Score;
            thesis.Content = await SaveFile(request.FileContent);
            thesis.FinishedAt = request.FinishedAt;
            thesis.MakedAt = request.MakedAt;
            thesis.ProtectedAt = request.ProtectedAt;
            thesis.IsProtected = request.IsProtected;


            var result = await _thesisCollection.ReplaceOneAsync(filter, thesis);
            return  (int)result.ModifiedCount;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
