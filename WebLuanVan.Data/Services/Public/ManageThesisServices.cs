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
                CreatedAt = DateTime.Now,
                FacultyId = request.FacultyId,
                LectureId = request.LectureId,
                Phase = request.Phase,
                StudentId = request.StudentId,
                Year = request.Year,
            };
            if(request.FileContent != null)
            {
                thesis.Content = await this.SaveFile(request.FileContent);
            }
            await _thesisCollection.InsertOneAsync(thesis);

            return 1;
        }

        public async Task<int> Delete(string thesisId)
        {
            var filter = Builders<Thesis>.Filter.Eq("thesisId", thesisId);
            await _thesisCollection.DeleteOneAsync(filter);
            return 1;
        }

        public async Task<List<Thesis>> Get()
        {
            var thesis = await _thesisCollection.FindSync(_ => true).ToListAsync();
            return thesis;
        }

        public async Task<Thesis> GetThesisById(string thesisId)
        {
            var thesis = await _thesisCollection.Find(x => x.ThesisId == thesisId).FirstOrDefaultAsync();
            return (Thesis)thesis;
        }

        public async Task<int> Update(ThesisRequest request)
        {
            var thesis = await GetThesisById(request.ThesisId);
            if(thesis == null)
            {
                throw new Exception($"Cannot find thesis with id{request.ThesisId}");
            }
            var filter = Builders<Thesis>.Filter.Eq("thesisId", request.ThesisId);
            //var json = new JavaScriptSerializer().Serialize(request);
            //request.Id = thesis.Id;
            //var update = request.ToBsonDocument();
            thesis.UpdatedAt = DateTime.Now;
            thesis.ThesisName = request.ThesisName;
            thesis.ThesisId = request.ThesisId;
            thesis.AcademicYear = request.AcademicYear;
            thesis.CreatedAt = DateTime.Now;
            thesis.FacultyId = request.FacultyId;
            thesis.LectureId = request.LectureId;
            thesis.Phase = request.Phase;
            thesis.StudentId = request.StudentId;
            thesis.Year = request.Year;
            var result = await _thesisCollection.ReplaceOneAsync(filter, thesis);
            return 1;
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
