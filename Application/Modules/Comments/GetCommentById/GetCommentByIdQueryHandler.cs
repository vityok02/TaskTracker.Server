﻿using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Comments.GetCommentById;

internal sealed class GetCommentByIdQueryHandler
    : IQueryHandler<GetCommentByIdQuery, CommentDto>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IMapper _mapper;

    public GetCommentByIdQueryHandler(ICommentRepository commentRepository, IMapper mapper)
    {
        _commentRepository = commentRepository;
        _mapper = mapper;
    }

    public async Task<Result<CommentDto>> Handle(
        GetCommentByIdQuery query,
        CancellationToken cancellationToken)
    {
        var comment = await _commentRepository
            .GetExtendedByIdAsync(query.CommentId);

        if (comment is null)
        {
            return Result<CommentDto>
                .Failure(CommentErrors.NotFound);
        }

        return Result<CommentDto>
            .Success(_mapper.Map<CommentDto>(comment));
    }
}
