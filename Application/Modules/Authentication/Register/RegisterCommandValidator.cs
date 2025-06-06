﻿using Application.Extensions;
using FluentValidation;

namespace Application.Modules.Authentication.Register;

internal class RegisterCommandValidator
    : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(x => x.Email)
            .ApplyEmailRules();

        RuleFor(x => x.Password)
            .ApplyPasswordRules()
            .Equal(x => x.ConfirmedPassword)
            .WithMessage("Passwords must match");
    }
}
