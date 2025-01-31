using Api.Controllers.Base;
using Api.Extensions;
using Api.Filters;
using Application.Modules.Members.AddMember;
using Application.Modules.Members.GetAllMembers;
using Application.Modules.Members.GetMember;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[ProjectMember]
[Route("projects/{projectId:guid}/members")]
public class ProjectMemberController : BaseController
{
    private Guid UserId => User.GetUserIdFromClaims();

    public ProjectMemberController(
        ISender sender,
        LinkGenerator linkGenerator)
        : base(sender, linkGenerator)
    {
    }

    [ProjectMember(Roles.Admin)]
    [HttpPost]
    [EndpointName(nameof(AddMember))]
    public async Task<IActionResult> AddMember(
        [FromRoute] Guid projectId,
        [FromBody] RoleRequest roleRequest)
    {
        var command = new AddMemberCommand(
            roleRequest.UserId,
            projectId,
            roleRequest.RoleId);

        var result = await Sender
            .Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("{memberId:guid}")]
    [EndpointName(nameof(GetMember))]
    public async Task<IActionResult> GetMember(
        [FromRoute] Guid projectId)
    {
        var query = new GetMemberQuery(
            UserId,
            projectId);

        var result = await Sender
            .Send(query);

        return result.IsFailure
            ? BadRequest(result.Error)
            : Ok(result.Value);
    }

    [HttpGet]
    [EndpointName(nameof(GetAllMembers))]
    public async Task<IActionResult> GetAllMembers(
        [FromRoute] Guid projectId)
    {
        var query = new GetAllMembersQuery(
            UserId,
            projectId);

        var result = await Sender
            .Send(query);

        return result.IsFailure
            ? BadRequest(result.Error)
            : Ok(result.Value);
    }
}
