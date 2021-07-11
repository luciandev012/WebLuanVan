using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Nest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.Services.Common;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request.Thesis;
using Word = Microsoft.Office.Interop.Word;
namespace WebLuanVan.Data.Services.Public
{
    public class ManageThesisServices : IManageThesisService
    {
        private readonly IMongoCollection<Thesis> _thesisCollection;
        private readonly IMongoCollection<ThesisData> _thesisDataCollection;
        private readonly IStorageService _storageService;
        private readonly IElasticClient _elasticClient;
        public ManageThesisServices(IMongoDatabase database, IStorageService storageService, IElasticClient elasticClient)
        {
            _thesisCollection = database.GetCollection<Thesis>("thesis");
            _thesisDataCollection = database.GetCollection<ThesisData>("thesis_data");
            _storageService = storageService;
            _elasticClient = elasticClient;


        }
        public async Task<int> Create(ThesisRequest request)
        {
            var arrDebateLecture = request.DebateLectureId.Trim().Split(',');

            var thesis = new Thesis()
            {
                ThesisName = request.ThesisName,
                ThesisId = request.ThesisId,
                AcademicYear = request.AcademicYear,
                MakedAt = request.MakedAt,
                FacultyId = request.FacultyId,
                GuideLectureId = request.GuideLectureId,
                DebateLectureId = arrDebateLecture,
                Phase = request.Phase,
                StudentId = request.StudentId,
                Year = request.Year,
                Language = request.Language,
                FinishedAt = request.FinishedAt,
                IsProtected = request.IsProtected,
                ProtectedAt = request.ProtectedAt,
                Score = request.Score
            };
            if (request.FileContent != null)
            {
                thesis.Content = await this.SaveFile(request.FileContent);
            }
            object missing = Missing.Value;
            object readOnly = true;
            object isvisible = true;
            object saveChange = false;
            object filename = Path.Combine(_storageService.GetFolder(), thesis.Content);
            if (File.Exists((string)filename))
            {
                Word.Application wordApp = new Word.Application();
                Word.Document myWordDoc = null;
                wordApp.Visible = false;
                myWordDoc = wordApp.Documents.Open(ref filename, ref missing, ref readOnly,
                                                   ref missing, ref missing, ref missing,
                                                   ref missing, ref missing, ref missing,
                                                   ref missing, ref missing, ref isvisible,
                                                   ref missing, ref missing, ref missing, ref missing);


                ReadFile value = new ReadFile();
                ThesisData thesisData = new ThesisData();
                thesisData.PageCount = myWordDoc.ComputeStatistics(Word.WdStatistic.wdStatisticPages, false).ToString();
                thesisData.University = value.GetString("trường", myWordDoc);
                thesisData.GuideLecture = value.GetString("hướng dẫn", myWordDoc);
                int id = value.Index;
                thesisData.ThesisName = value.GetStringBefore(id, myWordDoc);
                thesisData.Student = value.GetRange("thực hiện", "lớp", myWordDoc);
                thesisData.Class = value.GetString("lớp", myWordDoc);
                thesisData.AcademicYear = value.GetString("khoá", myWordDoc);

                thesisData.DebateLecture = value.GetArrayByString("phản biện", myWordDoc);
                thesisData.Comment = value.GetStringAfter(new string[] { "nhận xét", "xác nhận" }, myWordDoc);
                thesisData.Recommend = value.GetStringAfter(new string[] { "kiến nghị", "đánh giá" }, myWordDoc);
                thesisData.Summary = value.GetSummay(new string[] { "tóm tắt", "tóm tắt" }, myWordDoc);
                Word.TableOfContents oneToC = myWordDoc.TablesOfContents[1];
                thesisData.Table = oneToC.Range.Text.ToString().Split('\r');
                value.Index += thesisData.Table.Length;
                thesisData.Chapter = value.GetChapter(myWordDoc);
                thesisData.ReferDoc = value.GetReferDoc(myWordDoc);
                thesisData.ProtectedDate = thesis.ProtectedAt.ToString("dd-MM-yyyy");
                thesisData.ThesisId = thesis.ThesisId;
                await _thesisDataCollection.InsertOneAsync(thesisData);
                wordApp.Quit(ref saveChange);
            }
            await _thesisCollection.InsertOneAsync(thesis);

            return 1;
        }

        public async Task<int> Delete(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var filter = Builders<Thesis>.Filter.Eq("_id", objId);
            var thesis = await GetThesisById(id);
            await _storageService.DeleteFileAsync(thesis.Content);
            var result = await _thesisCollection.DeleteOneAsync(filter);

            return (int)result.DeletedCount;
        }

        public async Task<ApiResult<PagedResult<ThesisViewModel>>> Get(ThesisPagingRequest request)
        {
            //if(request.Field == null)
            //{
            //    request.Field = "thesisName";
            //}
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
            if (string.IsNullOrEmpty(request.LanguageId))
            {
                request.LanguageId = "";
            }
            //var filter = Builders<Thesis>.Filter.Eq(x => x.ThesisName.ToLower().Contains(request.Keyword));
            var result = new List<Thesis>();
            int totalRow = 0;
            if (!string.IsNullOrEmpty(request.Keyword) && !string.IsNullOrEmpty(request.Class) && !string.IsNullOrEmpty(request.StudentCode) && !string.IsNullOrEmpty(request.AcademicYear.ToString()))
            {
                var response = await _elasticClient.SearchAsync<ThesisData>(
                s => s.Query(q => q.Bool(b => b.
                            Must(mu => mu.
                                Match(m => m.Field(f => f.ThesisName).Query(request.Keyword)), mu => mu.
                                Match(m => m.Field(f => f.Student).Query(request.StudentCode)), mu => mu.
                                Match(m => m.Field(f => f.Class).Query(request.Class)), mu => mu.
                                Match(m => m.Field(f => f.AcademicYear).Query(request.AcademicYear.ToString()))
                            )
                        )
                    )
                );

                var res = response?.Documents?.ToList();
                totalRow = (int)res.Count;

                foreach (var r in res)
                {
                    var t = await _thesisCollection.Find(x => x.ThesisId == r.ThesisId && x.Language == request.LanguageId && x.FacultyId == request.Faculty).FirstOrDefaultAsync();
                    result.Add(t);
                }
            }
            else
            {
                result = await _thesisCollection.Find(_ => true).ToListAsync();
                totalRow = result.Count;
            }
            result = result.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
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



        public async Task<ThesisViewModel> GetThesisById(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var item = await _thesisCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            var result = new ThesisViewModel()
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
            return result;
        }

        public async Task<int> Update(ThesisRequest request)
        {
            ObjectId objId = ObjectId.Parse(request.Id);
            var thesis = await _thesisCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            if (thesis == null)
            {
                throw new Exception($"Cannot find thesis with id{objId.ToString()}");
            }
            var filter = Builders<Thesis>.Filter.Eq("Id", objId);
            await _storageService.DeleteFileAsync(thesis.Content);
            var arrDebateLecture = request.DebateLectureId.Trim().Split(',');
            thesis.ThesisName = request.ThesisName;
            thesis.ThesisId = request.ThesisId;
            thesis.AcademicYear = request.AcademicYear;
            thesis.FacultyId = request.FacultyId;
            thesis.GuideLectureId = request.GuideLectureId;
            thesis.Phase = request.Phase;
            thesis.StudentId = request.StudentId;
            thesis.Year = request.Year;
            thesis.Language = request.Language;
            thesis.DebateLectureId = arrDebateLecture;
            thesis.Score = request.Score;
            thesis.Content = await SaveFile(request.FileContent);
            thesis.FinishedAt = request.FinishedAt;
            thesis.MakedAt = request.MakedAt;
            thesis.ProtectedAt = request.ProtectedAt;
            var result = await _thesisCollection.ReplaceOneAsync(filter, thesis);

            return (int)result.ModifiedCount;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Common.Common.ConvertToUnSign(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
        public async Task<bool> ChangeThesisStatus(string id)
        {
            ObjectId objId = ObjectId.Parse(id);
            var thesis = await _thesisCollection.Find(x => x.Id == objId).FirstOrDefaultAsync();
            if (thesis == null)
            {
                return false;
            }
            bool status = thesis.IsProtected ? false : true;
            var filter = Builders<Thesis>.Filter.Eq("_id", objId);
            var update = Builders<Thesis>.Update.Set("isProtected", status);
            var result = await _thesisCollection.UpdateOneAsync(filter, update);
            return result.ModifiedCount > 0;
        }
    }
}