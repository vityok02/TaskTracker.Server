using FluentValidation;

namespace Application.Modules.Projects.CreateProject;

public class CreateProjectCommandValidator
    : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
