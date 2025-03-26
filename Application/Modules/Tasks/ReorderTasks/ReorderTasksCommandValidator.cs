using FluentValidation;

namespace Application.Modules.Tasks.ReorderTasks;

public class ReorderTasksCommandValidator
    : AbstractValidator<ReorderTasksCommand>
{
    public ReorderTasksCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .NotEmpty()
            .NotEqual(x => x.BeforeTaskId.GetValueOrDefault());
    }
}
