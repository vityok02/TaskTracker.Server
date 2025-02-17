using Application.Extensions;
using FluentValidation;

namespace Application.Modules.Identity.ChangePassword;

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
