using Api.Controllers.Base;
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
    public async Task<IActionResult> GetAllRoles()
    {
        var result = await Sender.Send(new GetAllRolesQuery());

        return result.IsFailure
            ? BadRequest(result.Error)
            : Ok(result.Value);
    }
}
