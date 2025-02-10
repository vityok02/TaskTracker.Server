using FluentValidation;

namespace Application.Extensions;

public static class PasswordValidationExtensions
{
    public static IRuleBuilder<T, string> ApplyPasswordRules<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .Length(8, 50)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.");
    }

    public static IRuleBuilder<T, string> ApplyEmailRules<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .EmailAddress();
    }
}
