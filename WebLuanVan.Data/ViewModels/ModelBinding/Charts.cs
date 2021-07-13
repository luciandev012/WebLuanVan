using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.ModelBinding
{
    public class Charts
    {
        public long TotalThesis { get; set; }
        public long ProtectedThesis { get; set; }
        public long NotProtectedThesis { get; set; }
        public long ThesisGreaterThan5 { get; set; }
    }
}
