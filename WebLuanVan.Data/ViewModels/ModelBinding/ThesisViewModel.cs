using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebLuanVan.Data.ViewModels.ModelBinding
{
    public class ThesisViewModel
    {

        public string Id { get; set; }
        [Display(Name = "Tên luận văn")]
        public string ThesisName { get; set; }
        [UIHint("DebateLecture")]
        [Display(Name = "Mã sinh viên")]
        [UIHint("Student")]
        public string[] StudentId { get; set; }
        [Display(Name = "Năm")]
        public int Year { get; set; }
        [Display(Name = "Đợt")]
        public int Phase { get; set; }
        [Display(Name = "Khoá")]
        public int AcademicYear { get; set; }
        [Display(Name = "Giảng viên hướng dẫn")]
        public string GuideLectureId { get; set; }
        [Display(Name = "Giảng viên phản biện")]
        [UIHint("DebateLecture")]
        public string[] DebateLectureId { get; set; }
        [Display(Name = "Mã khoa")]
        public string FacultyId { get; set; }
        [Display(Name = "File luận văn")]
        public string Content { get; set; }
        [Display(Name = "Mã luận văn")]
        public string ThesisId { get; set; }
        [Display(Name = "Ngôn ngữ")]
        public string Language { get; set; }
        [Display(Name = "Ngày bảo vệ")]
        public DateTime ProtectedAt { get; set; }
        [Display(Name = "Ngày thực hiện")]
        public DateTime MakedAt { get; set; }
        [Display(Name = "Ngày hoàn thành")]
        public DateTime FinishedAt { get; set; }
        [Display(Name = "Điểm")]
        public int Score { get; set; }
        [UIHint("IsActive")]
        public bool IsProtected { get; set; }
    }
}
