using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.Common
{
    public class PageViewModel : RequestBase
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
