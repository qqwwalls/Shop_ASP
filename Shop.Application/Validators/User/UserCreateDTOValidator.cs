using FluentValidation;
using Shop.Application.DTOs.UserDTOs;

namespace Shop.Application.Validators.User
{
    public class UserCreateDTOValidator : AbstractValidator<UserCreateDTO>
    {
        public UserCreateDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(5).WithMessage("Password is too short");
        }
    }
}
