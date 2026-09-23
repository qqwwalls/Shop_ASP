using FluentValidation;
using Shop.Application.DTOs;

namespace Shop.Application.Validators.Category
{
    public class CategoryCreateDTOValidator : AbstractValidator<CategoryCreateDTO>
    {
        public CategoryCreateDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва категорії є обов'язковою")
                .MaximumLength(100).WithMessage("Назва не може бути довшою за 100 символів");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug є обов'язковим")
                .MaximumLength(100).WithMessage("Slug не може бути довшим за 100 символів");
        }
    }
}
