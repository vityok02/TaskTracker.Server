using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Comments.DeleteComment;

public sealed record DeleteCommentCommand(
    Guid CommentId,
    Guid UserId)
    : ICommand;

internal sealed class DeleteCommentCommandHandler
    : ICommandHandler<DeleteCommentCommand>
{
    private readonly ICommentRepository _commentRepository;

    public DeleteCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Result> Handle(
        DeleteCommentCommand command,
        CancellationToken cancellationToken)
    {
        var comment = await _commentRepository
            .GetByIdAsync(command.CommentId);

        if (comment is null)
        {
            return Result
                .Failure(CommentErrors.NotFound);
        }

        if (comment.CreatedBy != command.UserId)
        {
            return Result
                .Failure(CommentErrors.Forbidden);
        }

        await _commentRepository
            .DeleteAsync(comment.Id);

        return Result.Success();
    }
}
