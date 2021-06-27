using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebLuanVan.Data.ViewModels.ModelBinding
{
    public class LectureViewModel
    {
       
        public string Id { get; set; }
        [Display(Name = "Mã giảng viên")]
        public string LectureId { get; set; }
        [Display(Name = "Tên giảng viên")]
        public string LectureName { get; set; }
        [Display(Name = "Chức vụ")]
        public string LectureRole { get; set; }
        
        public string Email { get; set; }

    }
}
