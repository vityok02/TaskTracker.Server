using Api.Controllers.Base;
using Application.Modules.Roles;
using Application.Modules.Roles.GetAllRoles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[Route("roles")]
public class RoleController : BaseController
{
    public RoleController(
        ISender sender,
        LinkGenerator linkGenerator) 
        : base(sender, linkGenerator)
    {
    }

    // Question: Who can see the roles?
    [HttpGet]
    [ProducesResponseType<IEnumerable<RoleResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoles(
        CancellationToken token)
    {
        var result = await Sender
            .Send(new GetAllRolesQuery(), token);

        return result.IsFailure
            ? HandlerFailure(result)
            : Ok(result.Value);
    }
}
