using FluentValidation;
using MainHW.Models.DTO;

namespace MainHW.Models.Validation;

public class OrderDtoValidator : AbstractValidator<OrderDto>
{
    public OrderDtoValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        RuleFor(x => x.TotalCost)
            .GreaterThan(0).WithMessage("Total cost must be greater than 0.");
        RuleFor(x => x.Comment)
            .MaximumLength(200).WithMessage("Comment must be at most 200 characters long.");
    }
}
