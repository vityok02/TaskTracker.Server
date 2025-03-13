using Api.Controllers.Abstract;
using Api.Controllers.ProjectMember.Requests;
using Api.Controllers.ProjectMember.Responses;
using Api.Extensions;
using Api.Filters;
using Application.Modules.Members.AddMember;
using Application.Modules.Members.DeleteMember;
using Application.Modules.Members.GetAllMembers;
using Application.Modules.Members.GetMember;
using Application.Modules.Members.UpdateMember;
using AutoMapper;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.ProjectMember;

[ProjectMember]
[Route("projects/{projectId:guid}/members")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
public class ProjectMemberController : BaseController
{
    private const string GetByIdAction = "GetProjectMemberById";

    public ProjectMemberController(
        ISender sender,
        IMapper mapper)
        : base(sender, mapper)
    {
    }

    [ProjectMember(Roles.Admin)]
    [HttpPost]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid projectId,
        [FromBody] MemberRequest roleRequest,
        CancellationToken token)
    {
        var command = new AddMemberCommand(
            projectId,
            roleRequest.UserId,
            roleRequest.RoleId);

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : CreatedAtAction(
                GetByIdAction,
                new { projectId, memberId = result.Value.UserId },
                Mapper.Map<ProjectMemberResponse>(result.Value));
    }

    [ProjectMember]
    [ActionName(GetByIdAction)]
    [HttpGet("{memberId:guid}")]
    [ProducesResponseType<ProjectMemberResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid memberId,
        CancellationToken token)
    {
        var query = new GetMemberQuery(
            memberId,
            projectId);

        var result = await Sender
            .Send(query, token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<ProjectMemberResponse>(result.Value));
    }

    [ProjectMember]
    [HttpGet]
    [ProducesResponseType<IEnumerable<ProjectMemberResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        [FromRoute] Guid projectId,
        CancellationToken token)
    {
        var query = new GetAllMembersQuery(
            User.GetUserId(),
            projectId);

        var result = await Sender
            .Send(query, token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<IEnumerable<ProjectMemberResponse>>(result.Value));
    }

    [ProjectMember(Roles.Admin)]
    [HttpPut("{memberId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task <IActionResult> UpdateAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid memberId,
        [FromBody] MemberRequest request,
        CancellationToken token)
    {
        var command = new UpdateMemberCommand(
            projectId,
            memberId,
            request.RoleId);

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }

    [ProjectMember(Roles.Admin)]
    [HttpDelete("{memberId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task <IActionResult> DeleteAsync(
        Guid projectId,
        Guid memberId,
        CancellationToken token)
    {
        var command = new DeleteMemberCommand(
            projectId,
            memberId,
            User.GetUserId());

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }
}
