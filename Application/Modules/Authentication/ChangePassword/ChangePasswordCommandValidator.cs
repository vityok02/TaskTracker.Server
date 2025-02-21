using Application.Extensions;
using Application.Modules.Authentication.ChangePassword;
using FluentValidation;

namespace Application.Modules.Authentication.ChangePassword;

public sealed class ChangePasswordCommandValidator
    : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .ApplyPasswordRules();

        RuleFor(x => x.NewPassword)
            .ApplyPasswordRules()
            .Equal(x => x.ConfirmedPassword)
            .WithMessage("Passwords do not match.");
    }
}
