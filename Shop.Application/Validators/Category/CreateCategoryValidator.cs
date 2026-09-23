using FluentValidation;
using Shop.Application.DTOs;

namespace Shop.Application.Validators.Category
{
    public class CreateCategoryValidator : AbstractValidator<CategoryCreateDTO>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва категорії є обов'язковою")
                .MaximumLength(100).WithMessage("Назва не може бути довшою за 100 символів");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug є обов'язковим")
                .MaximumLength(100).WithMessage("Slug не може бути довшим за 100 символів")
                .Matches("^[a-zA-Z0-9-]+$").WithMessage("Slug має містити лише латинські літери, цифри та дефіси");
        }
    }
}
