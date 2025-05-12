using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Shared;

namespace Application.Modules.Tags.GetAllTags;

internal sealed class GetAllTagsQueryHandler
    : IQueryHandler<GetAllTagsQuery, IEnumerable<TagDto>>
{
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;

    public GetAllTagsQueryHandler(
        ITagRepository tagRepository,
        IMapper mapper)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TagDto>>> Handle(
        GetAllTagsQuery request,
        CancellationToken cancellationToken)
    {
        var tags = await _tagRepository
            .GetAllAsync(request.ProjectId);

        return Result<IEnumerable<TagDto>>.Success(
            _mapper.Map<IEnumerable<TagDto>>(tags.OrderBy(t => t.SortOrder)));
    }
}
