using FluentValidation;
using MainHW.Models.DTO;

namespace MainHW.Models.Validation;

public class MessageAddRequestValidator : AbstractValidator<MessageAddRequest>
{
    public MessageAddRequestValidator()
    {
        RuleFor(x => x.From)
            .NotEmpty().WithMessage("From is required.")
            .MinimumLength(3).WithMessage("From must be at least 3 characters long.")
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Text is required.")
            .MinimumLength(1).WithMessage("Text must be at least 1 character long.");
    }
}
