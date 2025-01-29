using Api.Base;
using Api.Users.Dtos;
using Application.Users.RegisterUser;
using Application.Users.GetUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Projects.CreateProject;

namespace Api.Users;
[Route("users")]
[ApiController]
public sealed class UserController : ApiController
{
    public UserController(ISender sender, LinkGenerator linkGenerator)
        : base(sender, linkGenerator)
    {
    }

    [HttpGet]
    [ActionName("GetUser")]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        var result = await Sender.Send(new GetUserQuery(id));

        return result.IsFailure
            ? NotFound(result.Error)
            : Ok(result.Value);
    }

    [HttpPost]
    [ActionName("RegisterUser")]
    [Route("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest userDto)
    {
        var result = await Sender.Send(new RegisterUserCommand(userDto));

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var uri = LinkGenerator.GetUriByAction(HttpContext, "GetUser", values: new { result.Value.Id });

        return Created(uri, result.Value);
    }
}
