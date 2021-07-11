using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.ModelBinding
{
    public class StudentValidator : AbstractValidator<StudentViewModel>
    {
        public StudentValidator()
        {
            RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Số điện thoại không được để trống");
            RuleFor(x => x.StudentName).NotEmpty().WithMessage("Tên sinh viên không được để trống");
        }
    }
}
