using System;
using System.Collections.Generic;
using System.Text;
using WebLuanVan.Data.ViewModels.Common;

namespace WebLuanVan.Data.ViewModels.Request.Thesis
{
    public class ThesisPagingRequest : PageViewModel
    {
        public string Keyword { get; set; } = " ";
        public string LanguageId { get; set; } = " ";
        public string StudentCode { get; set; } = " ";
        public string Class { get; set; } = " ";
        public string Faculty { get; set; } = " ";
        public string AcademicYear { get; set; } = " ";
        public string Major { get; set; } = " ";
        public string Role { get; set; } = " ";
    }
}
