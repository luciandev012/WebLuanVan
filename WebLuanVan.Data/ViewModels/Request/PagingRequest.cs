using System;
using System.Collections.Generic;
using System.Text;
using WebLuanVan.Data.ViewModels.Common;

namespace WebLuanVan.Data.ViewModels.Request
{
    public class PagingRequest : PageViewModel
    {
        public string Keyword { get; set; }
    }
}
