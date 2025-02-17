using Api.Controllers.Base;
using Api.Extensions;
using Api.Filters;
using Application.Modules.Members;
using Application.Modules.Members.AddMember;
using Application.Modules.Members.GetAllMembers;
using Application.Modules.Members.GetMember;
using AutoMapper;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ProjectMember]
[Route("projects/{projectId:guid}/members")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
public class ProjectMemberController : BaseController
{
    private Guid UserId => User.GetUserIdFromClaims();

    public ProjectMemberController(
        ISender sender,
        LinkGenerator linkGenerator,
        IMapper mapper)
        : base(sender, linkGenerator, mapper)
    {
    }

    [ProjectMember(Roles.Admin)]
    [HttpPost]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddMember(
        [FromRoute] Guid projectId,
        [FromBody] RoleRequest roleRequest,
        CancellationToken token)
    {
        var command = new AddMemberCommand(
            roleRequest.UserId,
            projectId,
            roleRequest.RoleId);

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : CreatedAtAction(
                nameof(GetMember),
                new { memberId = result.Value.UserId },
                result.Value);
    }

    [HttpGet("{memberId:guid}", Name = nameof(GetMember))]
    [ProducesResponseType<MemberResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetMember(
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
            : Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<MemberResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllMembers(
        [FromRoute] Guid projectId,
        CancellationToken token)
    {
        var query = new GetAllMembersQuery(
            UserId,
            projectId);

        var result = await Sender
            .Send(query, token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(result.Value);
    }
}
