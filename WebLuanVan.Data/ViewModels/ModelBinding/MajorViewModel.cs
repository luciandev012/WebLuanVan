using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebLuanVan.Data.ViewModels.ModelBinding
{
    public class MajorViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Tên ngành")]
        public string MajorName { get; set; }
        [Display(Name = "Mã ngành")]
        public string MajorId { get; set; }
        [Display(Name = "Mã khoa")]
        public string FacultyId { get; set; }
    }
}
