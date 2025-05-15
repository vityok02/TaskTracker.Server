using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Application.Modules.Tags.CreateTag;
using AutoMapper;
using Domain.Constants;
using Domain.Entities;
using Domain.Shared;

namespace Application.Modules.Tags;

internal sealed class CreateTagCommandHandler
    : ICommandHandler<CreateTagCommand, TagDto>
{
    private readonly ITagRepository _tagRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMapper _mapper;

    public CreateTagCommandHandler(
        ITagRepository tagRepository,
        IDateTimeProvider dateTimeProvider,
        IMapper mapper)
    {
        _tagRepository = tagRepository;
        _dateTimeProvider = dateTimeProvider;
        _mapper = mapper;
    }

    public async Task<Result<TagDto>> Handle(
        CreateTagCommand command,
        CancellationToken cancellationToken)
    {
        var tags = await _tagRepository
            .GetAllAsync(command.ProjectId);

        var tag = new TagEntity()
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Color = command.Color ?? Colors.Default,
            SortOrder = tags
                .Select(x => x.SortOrder)
                .DefaultIfEmpty(0)
                .Max() + 1,
            ProjectId = command.ProjectId,
            CreatedBy = command.UserId,
            CreatedAt = _dateTimeProvider.GetCurrentTime()
        };

        var id = await _tagRepository
            .CreateAsync(tag);

        return Result<TagDto>.Success(
            _mapper.Map<TagDto>(tag));
    }
}