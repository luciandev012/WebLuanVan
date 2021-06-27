using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebLuanVan.Data.Entity;
using WebLuanVan.Data.ViewModels.Common;
using WebLuanVan.Data.ViewModels.ModelBinding;
using WebLuanVan.Data.ViewModels.Request;
using WebLuanVan.Data.ViewModels.Request.Thesis;

namespace WebLuanVan.AdminApp.Services
{
    public class ThesisApiClient : IThesisApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ThesisApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Create(ThesisRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var requestContent = new MultipartFormDataContent();
            if(request.FileContent != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.FileContent.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.FileContent.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "fileContent", request.FileContent.FileName);
            }
            requestContent.Add(new StringContent(request.ThesisName), "thesisName");
            requestContent.Add(new StringContent(request.StudentId), "studentId");
            requestContent.Add(new StringContent(request.Year.ToString()), "year");
            requestContent.Add(new StringContent(request.Phase.ToString()), "phase");
            requestContent.Add(new StringContent(request.AcademicYear.ToString()), "academicYear");
            requestContent.Add(new StringContent(request.GuideLectureId), "guideLectureId");
            requestContent.Add(new StringContent(request.DebateLectureId.ToString()), "debateLectureId");
            requestContent.Add(new StringContent(request.FacultyId), "facultyId");
            requestContent.Add(new StringContent(request.ThesisId), "thesisId");
            requestContent.Add(new StringContent(request.Language), "language");
            requestContent.Add(new StringContent(request.ProtectedAt.ToString()), "protectedAt");
            requestContent.Add(new StringContent(request.MakedAt.ToString()), "makedAt");
            requestContent.Add(new StringContent(request.FinishedAt.ToString()), "finishedAt");
            requestContent.Add(new StringContent(request.Score.ToString()), "score");
            requestContent.Add(new StringContent(request.IsProtected.ToString()), "isProtected");
            var response = await client.PostAsync($"/api/thesis/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<PagedResult<ThesisViewModel>> GetThesisPaging(ThesisPagingRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/thesis?pageIndex={request.PageIndex}&pageSize={request.PageSize}&keyword={request.Keyword}&languageId={request.LanguageId}");
            var body = await response.Content.ReadAsStringAsync();
            var thesis = JsonConvert.DeserializeObject<PagedResult<ThesisViewModel>>(body);
            return thesis;
        }
        public List<Language> GetLanguages()
        {
            List<Language> languages = new List<Language>();
            Language vi = new Language("vi", "Tiếng Việt");
            languages.Add(vi);
            Language en = new Language("en", "Tiếng Anh");
            languages.Add(en);
            return languages;
        }

        public async Task<bool> Update(ThesisRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var requestContent = new MultipartFormDataContent();
            if (request.FileContent != null)
            {
                byte[] data;
                using (var br = new BinaryReader(request.FileContent.OpenReadStream()))
                {
                    data = br.ReadBytes((int)request.FileContent.OpenReadStream().Length);
                }
                ByteArrayContent bytes = new ByteArrayContent(data);
                requestContent.Add(bytes, "fileContent", request.FileContent.FileName);
            }
            requestContent.Add(new StringContent(request.Id), "id");
            requestContent.Add(new StringContent(request.ThesisName), "thesisName");
            requestContent.Add(new StringContent(request.StudentId), "studentId");
            requestContent.Add(new StringContent(request.Year.ToString()), "year");
            requestContent.Add(new StringContent(request.Phase.ToString()), "phase");
            requestContent.Add(new StringContent(request.AcademicYear.ToString()), "academicYear");
            requestContent.Add(new StringContent(request.GuideLectureId), "guideLectureId");
            requestContent.Add(new StringContent(request.DebateLectureId.ToString()), "debateLectureId");
            requestContent.Add(new StringContent(request.FacultyId), "facultyId");
            requestContent.Add(new StringContent(request.ThesisId), "thesisId");
            requestContent.Add(new StringContent(request.Language), "language");
            requestContent.Add(new StringContent(request.ProtectedAt.ToString()), "protectedAt");
            requestContent.Add(new StringContent(request.MakedAt.ToString()), "makedAt");
            requestContent.Add(new StringContent(request.FinishedAt.ToString()), "finishedAt");
            requestContent.Add(new StringContent(request.Score.ToString()), "score");
            
            var response = await client.PutAsync($"/api/thesis/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<ThesisViewModel> GetThesisById(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var response = await client.GetAsync($"/api/thesis/{id}");
            var body = await response.Content.ReadAsStringAsync();
            var thesis = JsonConvert.DeserializeObject<ThesisViewModel>(body);
            
            return thesis;
        }
        public async Task<bool> Delete(string id)
        {
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var response = await client.DeleteAsync($"/api/thesis/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            return true;
        }
        public async Task<bool> Status(string id)
        {
            var json = JsonConvert.SerializeObject("");
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            var session = _httpContextAccessor.HttpContext.Session.GetString("Token");
            client.BaseAddress = new Uri("https://localhost:5001");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", session);
            var response = await client.PutAsync($"/api/thesis/{id}/status", httpContent);        
            return response.IsSuccessStatusCode;
        }
    }
}
