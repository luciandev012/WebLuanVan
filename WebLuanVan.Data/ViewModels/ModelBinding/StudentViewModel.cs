using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebLuanVan.Data.ViewModels.ModelBinding
{
    public class StudentViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Mã khoa")]
        public string FacultyId { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Tên sinh viên")]
        public string StudentName { get; set; }
        [Display(Name = "Mã sinh viên")]
        public string StudentId { get; set; }
        [Display(Name = "Mã ngành")]
        public string MajorId { get; set; }
    }
}
