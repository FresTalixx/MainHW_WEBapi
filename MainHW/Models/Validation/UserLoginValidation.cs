using FluentValidation;
using MainHW.Models.DTO;
using MainHW.Repositories;

namespace MainHW.Models.Validation;

public class UserLoginValidation : AbstractValidator<UserLoginRequest>
{
    private UserRepository _userRepository;

    public UserLoginValidation(UserRepository userRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is not valid.")

            .MustAsync(async (email, cancellationToken) => !await DoesUserExist(email, cancellationToken))
            .WithMessage("Email is not used.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Length(6, 100).WithMessage("Password must be between 6 and 100 characters.");
    }

    private async Task<bool> DoesUserExist(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null;
    }
}
