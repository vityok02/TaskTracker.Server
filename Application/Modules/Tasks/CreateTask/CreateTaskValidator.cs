using FluentValidation;

namespace Application.Modules.Tasks.CreateTask;

public class CreateTaskValidator
    : AbstractValidator<CreateTaskCommand>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
