using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.ModelBinding
{
    public class LectureValidator : AbstractValidator<LectureViewModel>
    {
        public LectureValidator()
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage("Email không đúng định dạng");
            RuleFor(x => x.LectureName).NotEmpty().WithMessage("Tên giảng viên không được để trống");
            RuleFor(x => x.LectureId).NotEmpty().WithMessage("Mã giảng viên không được để trống");
            //RuleFor(x => x.)
        }
    }
}
