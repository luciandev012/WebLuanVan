using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.Entity
{
    public class Language
    {
        public string LanguageId { get; set; }
        public string Name { get; set; }
        public Language(string id, string name)
        {
            LanguageId = id;
            Name = name;
        }
    }
}
