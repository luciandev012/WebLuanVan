using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.ModelBinding
{
    public class MajorValidator : AbstractValidator<MajorViewModel>
    {
        public MajorValidator()
        {
            RuleFor(x => x.MajorName).NotEmpty().WithMessage("Tên ngành không được để trống");
            RuleFor(x => x.MajorId).NotEmpty().WithMessage("Mã ngành không được để trống");
        }
    }
}
