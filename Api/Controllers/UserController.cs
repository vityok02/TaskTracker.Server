using Api.Controllers.Base;
using Application.Modules.Users.GetAllUsers;
using Application.Modules.Users.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("users")]
[ApiController]
public sealed class UserController : BaseController
{
    public UserController(
        ISender sender,
        LinkGenerator linkGenerator)
        : base(sender, linkGenerator)
    {
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [EndpointName(nameof(GetUser))]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        var result = await Sender
            .Send(new GetUserQuery(id));

        return result.IsFailure
            ? NotFound(result.Error)
            : Ok(result.Value);
    }

    [HttpGet]
    [EndpointName(nameof(GetAllUsers))]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await Sender
            .Send(new GetAllUsersQuery());

        return result.IsFailure
            ? BadRequest(result.Error)
            : Ok(result.Value);
    }
}
