using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tags.GetTagById;

internal sealed class GetTagByIdQueryHandler
    : IQueryHandler<GetTagByIdQuery, TagDto>
{
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;

    public GetTagByIdQueryHandler(
        ITagRepository tagRepository,
        IMapper mapper)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<Result<TagDto>> Handle(
        GetTagByIdQuery query,
        CancellationToken cancellationToken)
    {
        var tag = await _tagRepository.GetByIdAsync(query.TagId);

        if (tag is null)
        {
            return Result<TagDto>
                .Failure(TagErrors.NotFound);
        }

        return Result<TagDto>.Success(
            _mapper.Map<TagDto>(tag));
    }
}
