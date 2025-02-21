using Application.Extensions;
using FluentValidation;

namespace Application.Modules.Authentication.Login;

internal class LoginCommandValidator
    : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .ApplyEmailRules();

        RuleFor(x => x.Password)
            .ApplyPasswordRules();
    }
}
