using Api.Controllers.Abstract;
using Api.Controllers.Tag.Requests;
using Api.Controllers.Tag.Responses;
using Api.Extensions;
using Api.Filters;
using Application.Modules.Tags.CreateTag;
using Application.Modules.Tags.DeleteTag;
using Application.Modules.Tags.GetAllTags;
using Application.Modules.Tags.GetTagById;
using Application.Modules.Tags.UpdateTag;
using AutoMapper;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Tag;

[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
[Route("/projects/{projectId:guid}/tags")]
public class TagController : BaseController
{
    private const string GetByIdAction = "GetTagById";

    public TagController(
        ISender sender,
        IMapper mapper)
        : base(sender, mapper)
    {
    }

    [ProjectMember(Roles.Contributor)]
    [HttpPost]
    [ProducesResponseType<TagResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid projectId,
        [FromBody] TagRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateTagCommand(
            request.Name,
            request.Color,
            projectId,
            User.GetUserId());

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : CreatedAtAction(
                GetByIdAction,
                new
                {
                    projectId = command.ProjectId,
                    tagId = result.Value.Id
                },
                Mapper.Map<TagResponse>(result.Value));
    }

    [ProjectMember]
    [HttpGet("{tagId:guid}")]
    [ActionName(GetByIdAction)]
    [ProducesResponseType<TagResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid tagId,
        CancellationToken cancellationToken)
    {
        var query = new GetTagByIdQuery(tagId);

        var result = await Sender
            .Send(query, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<TagResponse>(result.Value));
    }

    [ProjectMember]
    [HttpGet]
    [ProducesResponseType<IEnumerable<TagResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        [FromRoute] Guid projectId,
        CancellationToken cancellationToken)
    {
        var query = new GetAllTagsQuery(projectId);

        var result = await Sender
            .Send(query, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<TagResponse[]>(result.Value));
    }

    [ProjectMember(Roles.Contributor)]
    [HttpPut("{tagId:guid}")]
    [ProducesResponseType<TagResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid tagId,
        [FromBody] TagRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTagCommand(
            tagId,
            request.Name,
            request.Color,
            User.GetUserId());

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<TagResponse>(result.Value));
    }

    [ProjectMember(Roles.Contributor)]
    [HttpDelete("{tagId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid tagId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteTagCommand(tagId);

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }
}
