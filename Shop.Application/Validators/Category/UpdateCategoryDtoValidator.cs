using FluentValidation;
using Shop.Application.DTOs;

namespace Shop.Application.Validators.Category
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required")
                .MaximumLength(100).WithMessage("Slug cannot exceed 100 characters")
                .Matches("^[a-zA-Z0-9-]+$").WithMessage("Slug must contain only latin letters, numbers, and hyphens");
        }
    }
}
