using FluentValidation;

namespace Application.Modules.Projects.UpdateProject;

public class UpdateProjectCommandValidator
    : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}
