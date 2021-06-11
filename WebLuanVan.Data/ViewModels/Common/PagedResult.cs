using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.Common
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecord { get; set; }
    }
}
