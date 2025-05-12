using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using Domain.Errors;
using Domain.Shared;

namespace Application.Modules.Tags.DeleteTag;

internal sealed class DeleteTagCommandHandler
    : ICommandHandler<DeleteTagCommand>
{
    private readonly ITagRepository _tagRepository;

    public DeleteTagCommandHandler(
        ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<Result> Handle(
        DeleteTagCommand request,
        CancellationToken cancellationToken)
    {
        var tag = await _tagRepository
            .GetByIdAsync(request.TagId);

        if (tag is null)
        {
            return Result
                .Failure(TagErrors.NotFound);
        }

        await _tagRepository
            .DeleteAsync(tag.Id);

        return Result.Success();
    }
}
