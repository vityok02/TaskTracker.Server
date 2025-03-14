using FluentValidation;

namespace Application.Modules.Comments.UpdateComment;

public class UpdateCommentCommandValidator
    : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.Comment)
            .NotEmpty();
    }
}
