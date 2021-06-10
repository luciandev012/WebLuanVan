using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.Request.Thesis
{
    public class ThesisRequest
    {
        public string ThesisName { get; set; }
        
        public string StudentId { get; set; }
        
        public int Year { get; set; }
        
        public int Phase { get; set; }
       
        public int AcademicYear { get; set; }
        
        public string LectureId { get; set; }
        
        public string FacultyId { get; set; }
        
        public IFormFile FileContent { get; set; }
        
        public string ThesisId { get; set; }
        public string Language { get; set; }
    }
}
