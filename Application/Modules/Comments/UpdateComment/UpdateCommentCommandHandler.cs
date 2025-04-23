using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Comments.UpdateComment;

internal sealed class UpdateCommentCommandHandler
    : ICommandHandler<UpdateCommentCommand, CommentDto>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public UpdateCommentCommandHandler(
        ICommentRepository commentRepository,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _commentRepository = commentRepository;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<Result<CommentDto>> Handle(
        UpdateCommentCommand command,
        CancellationToken cancellationToken)
    {
        var commentEntity = await _commentRepository
            .GetByIdAsync(command.CommentId);

        if (commentEntity is null)
        {
            return Result<CommentDto>
                .Failure(CommentErrors.NotFound);
        }

        if (commentEntity.TaskId != command.TaskId)
        {
            return Result<CommentDto>
                .Failure(CommentErrors.Forbidden);
        }

        if (commentEntity.CreatedBy != command.UserId)
        {
            return Result<CommentDto>
                .Failure(CommentErrors.Forbidden);
        }

        commentEntity.Comment = command.Comment;
        commentEntity.UpdatedBy = command.UserId;
        commentEntity.UpdatedAt = _dateTimeProvider.GetCurrentTime();

        await _commentRepository
            .UpdateAsync(commentEntity);

        var commentModel = await _commentRepository
            .GetExtendedByIdAsync(command.CommentId);

        return Result<CommentDto>
            .Success(_mapper.Map<CommentDto>(commentModel));
    }
}