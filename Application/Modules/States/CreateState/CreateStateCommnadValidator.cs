using FluentValidation;

namespace Application.Modules.States.CreateState;

public class CreateStateCommnadValidator
    : AbstractValidator<CreateStateCommand>
{
    public CreateStateCommnadValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
