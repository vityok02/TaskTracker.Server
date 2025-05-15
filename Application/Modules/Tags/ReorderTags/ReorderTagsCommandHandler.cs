using Application.Abstract.Interfaces;
using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Application.Extensions;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tags.ReorderTags;

internal sealed class ReorderTagsCommandHandler
    : ICommandHandler<ReorderTagsCommand>
{
    private readonly ITagRepository _tagRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ReorderTagsCommandHandler(
        ITagRepository tagRepository,
        IDateTimeProvider dateTimeProvider)
    {
        _tagRepository = tagRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(
        ReorderTagsCommand command,
        CancellationToken cancellationToken)
    {
        var tag = await _tagRepository
            .GetByIdAsync(command.TagId);

        if (tag is null)
        {
            return Result
                .Failure(TagErrors.NotFound);
        }

        var tags = (await _tagRepository
            .GetAllAsync(command.ProjectId))
            .OrderBy(s => s.SortOrder)
            .ToList();

        tags.RemoveAll(s => s.Id == command.TagId);

        tag.UpdatedBy = command.UserId;
        tag.UpdatedAt = _dateTimeProvider.GetCurrentTime();

        tags.InsertInOrderedList(command.BeforeTagId, tag);
        tags.Reorder();

        await _tagRepository.UpdateRangeAsync(tags);

        return Result.Success();
    }
}