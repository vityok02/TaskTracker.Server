using FluentValidation;

namespace Application.Modules.Comments.CreateComment;

public class CreateCommentCommandValidator
    : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty();
    }
}
