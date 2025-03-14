using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Comments.UpdateComment;

internal sealed class UpdateCommentCommandHandler
    : ICommandHandler<UpdateCommentCommand>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateCommentCommandHandler(
        ICommentRepository commentRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _commentRepository = commentRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        UpdateCommentCommand command,
        CancellationToken cancellationToken)
    {
        var commentEntity = await _commentRepository
            .GetByIdAsync(command.CommentId);

        if (commentEntity is null)
        {
            return Result
                .Failure(CommentErrors.NotFound);
        }

        if (commentEntity.CreatedBy != command.UserId)
        {
            return Result
                .Failure(CommentErrors.Forbidden);
        }

        commentEntity.Comment = command.Comment;
        commentEntity.UpdatedBy = command.UserId;
        commentEntity.UpdatedAt = _dateTimeProvider.GetCurrentTime();

        await _commentRepository
            .UpdateAsync(commentEntity);

        return Result.Success();
    }
}