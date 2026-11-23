using FluentValidation;
using MainHW.Models.DTO;

namespace MainHW.Models.Validation;

public class CategoryDtoValidator : AbstractValidator<CategoryDto>
{
    public CategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters long.");
    }
}
