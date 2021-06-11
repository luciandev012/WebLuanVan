using System;
using System.Collections.Generic;
using System.Text;
using WebLuanVan.Data.ViewModels.Common;

namespace WebLuanVan.Data.ViewModels.Request.Users
{
    public class GetUserPagingRequest : PageViewModel
    {
        public string  Keyword { get; set; }
    }
}
