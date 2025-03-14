using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Entities;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Comments.CreateComment;

internal sealed class CreateCommentCommandHandler
    : ICommandHandler<CreateCommentCommand, CommentDto>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ITaskRepository _taskRepository;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateCommentCommandHandler(
        ICommentRepository commentRepository,
        ITaskRepository taskRepository,
        IMapper mapper,
        IDateTimeProvider dateTimeProvider)
    {
        _commentRepository = commentRepository;
        _taskRepository = taskRepository;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<CommentDto>> Handle(
        CreateCommentCommand command,
        CancellationToken cancellationToken)
    {
        var task = await _taskRepository
            .GetExtendedByIdAsync(command.TaskId);

        if (task is null)
        {
            return Result<CommentDto>
                .Failure(TaskErrors.NotFound);
        }

        var commentEntity = new CommentEntity()
        {
            Id = Guid.NewGuid(),
            Comment = command.Comment,
            CreatedBy = command.UserId,
            TaskId = command.TaskId,
            CreatedAt = _dateTimeProvider.GetCurrentTime()
        };

        var commandId = await _commentRepository
            .CreateAsync(commentEntity);

        var commentModel = await _commentRepository
            .GetByIdExtendedAsync(commandId);

        return Result<CommentDto>
            .Success(_mapper.Map<CommentDto>(commentModel));
    }
}
