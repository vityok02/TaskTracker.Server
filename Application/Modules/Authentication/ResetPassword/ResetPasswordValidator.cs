using Application.Extensions;
using FluentValidation;

namespace Application.Modules.Authentication.ResetPassword;

public class ResetPasswordCommandValidator
    : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .ApplyEmailRules();
    }
}
