using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.Request.Thesis
{
    public class ThesisValidator : AbstractValidator<ThesisRequest>
    {
        public ThesisValidator()
        {
            RuleFor(x => x.ThesisName).NotEmpty().WithMessage("Tên luận văn không được để trống");
            RuleFor(x => x.ThesisId).NotEmpty().WithMessage("Mã luận văn không được để trống");
            RuleFor(x => x.StudentId).NotEmpty().WithMessage("Mã sinh viên không được để trống");
            RuleFor(x => x.FacultyId).NotEmpty().WithMessage("Mã ngành không được để trống");
            RuleFor(x => x.GuideLectureId).NotEmpty().WithMessage("Giảng viên hướng dẫn không được để trống");
            RuleFor(x => x.Score).LessThanOrEqualTo(10).WithMessage("Điểm không được quá 10")
                .GreaterThanOrEqualTo(0).WithMessage("Điểm không được nhỏ hơn 0");
            //RuleFor(x => x.StudentId).NotEmpty().WithMessage("Mã sinh viên không được để trống");
            //RuleFor(x => x.StudentId).NotEmpty().WithMessage("Mã sinh viên không được để trống");
            //RuleFor(x => x.StudentId).NotEmpty().WithMessage("Mã sinh viên không được để trống");
        }
    }
}
