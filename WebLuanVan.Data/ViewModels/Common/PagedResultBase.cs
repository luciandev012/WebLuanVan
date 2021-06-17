using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.Common
{
    public class PagedResultBase
    {
        public int TotalRecord { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int PageCount { 
            get 
            {
                var pageCount = (double)TotalRecord / PageSize;
                return (int)Math.Ceiling(pageCount);
            } 
        }
    }
}
