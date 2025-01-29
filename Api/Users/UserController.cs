using Api.Base;
using Api.Users.Dtos;
using Application.Users.GetUser;
using Application.Users.Login;
using Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Users;
[Route("users")]
[ApiController]
public sealed class UserController : ApiController
{
    public UserController(
        ISender sender,
        LinkGenerator linkGenerator)
        : base(sender, linkGenerator)
    {
    }

    [Authorize]
    [HttpGet]
    [ActionName("GetUser")]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        var result = await Sender
            .Send(new GetUserQuery(id));

        return result.IsFailure
            ? NotFound(result.Error)
            : Ok(result.Value);
    }

    [HttpPost]
    [ActionName("Register")]
    [Route("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterRequest userDto)
    {
        var result = await Sender
            .Send(new RegisterUserCommand(userDto));

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var uri = LinkGenerator
            .GetUriByAction(HttpContext, "GetUser", values: new { result.Value.Id });

        return Created(uri, result.Value);
    }

    [HttpPost]
    [ActionName("Login")]
    [Route("login")]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginRequest loginUserRequest)
    {
        var result = await Sender
            .Send(new LoginCommand(loginUserRequest.Email));

        return result.IsFailure
            ? Unauthorized(result.Error)
            : Ok(result.Value);
    }
}
