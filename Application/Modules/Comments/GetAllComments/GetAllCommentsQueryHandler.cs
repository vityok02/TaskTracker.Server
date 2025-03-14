using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Shared;

namespace Application.Modules.Comments.GetAllComments;

internal sealed class GetAllCommentsQueryHandler
    : IQueryHandler<GetAllCommentsQuery, IEnumerable<CommentDto>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public GetAllCommentsQueryHandler(
        ICommentRepository commentRepository,
        IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<CommentDto>>> Handle(
        GetAllCommentsQuery request,
        CancellationToken cancellationToken)
    {
        var comments = await _commentRepository
            .GetAllExtendedByTaskIdAsync(request.TaskId);

        return Result<IEnumerable<CommentDto>>
            .Success(_mapper.Map<IEnumerable<CommentDto>>(comments));
    }
}
