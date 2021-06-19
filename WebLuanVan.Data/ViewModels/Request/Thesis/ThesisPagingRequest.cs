using System;
using System.Collections.Generic;
using System.Text;
using WebLuanVan.Data.ViewModels.Common;

namespace WebLuanVan.Data.ViewModels.Request.Thesis
{
    public class ThesisPagingRequest : PageViewModel
    {
        public string Keyword { get; set; }
        public string Field { get; set; }
    }
}
