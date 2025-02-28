using FluentValidation;

namespace Application.Modules.Tasks.UpdateTask;

public class UpdateTaskCommandValidator
    : AbstractValidator<UpdateTaskCommand>
{
    public UpdateTaskCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
