using Api.Controllers.Base;
using Application.Modules.Users.Identity.Login;
using Application.Modules.Users.Identity.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("users")]
public class IdentityController : BaseController
{
    public IdentityController(
        ISender sender,
        LinkGenerator linkGenerator)
        : base(sender, linkGenerator)
    {
    }

    [HttpPost("register")]
    [EndpointName(nameof(RegisterUser))]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterRequest userDto)
    {
        var result = await Sender
            .Send(new RegisterCommand(userDto));

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var uri = LinkGenerator
            .GetPathByName(HttpContext,
            "GetUser",
            values: new { result.Value.Id });

        return Created(uri, result.Value);
    }

    [HttpPost("login")]
    [EndpointName(nameof(LoginUser))]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginRequest loginUserRequest)
    {
        var result = await Sender
            .Send(new LoginCommand(loginUserRequest));

        return result.IsFailure
            ? Unauthorized(result.Error)
            : Ok(result.Value);
    }
}
