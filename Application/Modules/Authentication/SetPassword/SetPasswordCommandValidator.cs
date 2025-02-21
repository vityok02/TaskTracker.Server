using Application.Extensions;
using FluentValidation;

namespace Application.Modules.Authentication.SetPassword;

public class SetPasswordCommandValidator
    : AbstractValidator<SetPasswordCommand>
{
    public SetPasswordCommandValidator()
    {
        RuleFor(x => x.Password)
            .ApplyPasswordRules()
            .Equal(x => x.ConfirmedPassword)
            .WithMessage("Passwords must match");
    }
}
