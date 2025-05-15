using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Constants;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tags.UpdateTag;

internal sealed class UpdateTagCommandHandler
    : ICommandHandler<UpdateTagCommand, TagDto>
{
    private readonly ITagRepository _tagRepository;
    private readonly IMapper _mapper;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UpdateTagCommandHandler(
        ITagRepository tagRepository,
        IMapper mapper,
        IDateTimeProvider dateTimeProvider)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<TagDto>> Handle(
        UpdateTagCommand command,
        CancellationToken cancellationToken)
    {
        var tag = await _tagRepository
            .GetByIdAsync(command.TagId);

        if (tag is null)
        {
            return Result<TagDto>
                .Failure(TagErrors.NotFound);
        }

        tag.Name = command.Name;
        tag.Color = string.IsNullOrWhiteSpace(command.Color)
            ? Colors.Default : command.Color;
        tag.UpdatedBy = command.UserId;
        tag.UpdatedAt = _dateTimeProvider
            .GetCurrentTime();

        await _tagRepository
            .UpdateAsync(tag);

        return Result<TagDto>
            .Success(_mapper.Map<TagDto>(tag));
    }
}
