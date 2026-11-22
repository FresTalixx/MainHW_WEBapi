using FluentValidation;
using MainHW.Models.DTO;
using MainHW.Repositories;

namespace MainHW.Models.Validation;

public class UserRegisterValidator : AbstractValidator<UserRegisterRequest>
{
    private UserRepository _userRepository;

    public UserRegisterValidator(UserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Username is required.")
            .Length(3, 100).WithMessage("Username must be between 3 and 100 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.")

            .MustAsync(async (email, cancellationToken) => !await DoesUserExist(email, cancellationToken))
            .WithMessage("Email is already in use.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(6, 100).WithMessage("Password must be between 6 and 100 characters.");
    }

    private async Task<bool> DoesUserExist(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user != null;
    }
}
