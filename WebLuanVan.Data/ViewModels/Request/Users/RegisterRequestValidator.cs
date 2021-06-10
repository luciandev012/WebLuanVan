using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLuanVan.Data.ViewModels.Request.Users
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required")
                .MaximumLength(20).WithMessage("Username can not over 20 characters");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Confirm password is required");
            RuleFor(x => x.ConfirmPassword).Equal(y => y.Password).WithMessage("Password and Confirm password are not match!");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required");
        }
    }
}
