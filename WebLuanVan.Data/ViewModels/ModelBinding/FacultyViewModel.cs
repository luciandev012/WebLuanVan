using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebLuanVan.Data.ViewModels.ModelBinding
{
    public class FacultyViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên khoa")]
        public string FacultyName { get; set; }
        [Display(Name = "Mã khoa")]
        public string FacultyId { get; set; }
        
    }
}
