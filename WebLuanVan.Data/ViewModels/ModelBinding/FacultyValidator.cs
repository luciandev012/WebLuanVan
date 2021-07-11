using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.ModelBinding
{
    public class FacultyValidator : AbstractValidator<FacultyViewModel>
    {
        public FacultyValidator()
        {
            RuleFor(x => x.FacultyName).NotEmpty().WithMessage("Tên ngành không được để trống");
            RuleFor(x => x.FacultyId).NotEmpty().WithMessage("Mã ngành không được để trống");
            
        }
    }
}
