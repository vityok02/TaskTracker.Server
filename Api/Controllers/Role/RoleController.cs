using Api.Controllers.Abstract;
using Api.Controllers.Role.Responses;
using Application.Modules.Roles.GetAllRoles;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Role;

[Authorize]
[Route("/projects/{projectId:guid}/roles")]
public class RoleController : BaseController
{
    public RoleController(
        ISender sender,
        IMapper mapper)
        : base(sender, mapper)
    {
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<RoleResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoles(
        CancellationToken token)
    {
        var result = await Sender
            .Send(new GetAllRolesQuery(), token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(result.Value);
    }
}
