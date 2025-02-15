using Api.Controllers.Base;
using Application.Modules.Users;
using Application.Modules.Users.GetAllUsers;
using Application.Modules.Users.GetUserById;
using AutoMapper;
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
        LinkGenerator linkGenerator,
        IMapper mapper)
        : base(sender, linkGenerator, mapper)
    {
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUser(
        [FromRoute] Guid id,
        CancellationToken token)
    {
        var result = await Sender
            .Send(new GetUserQuery(id), token);

        return result.IsFailure
            ? HandlerFailure(result)
            : Ok(result.Value);
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<UserResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers(
        CancellationToken token)
    {
        var result = await Sender
            .Send(new GetAllUsersQuery(), token);

        return result.IsFailure
            ? HandlerFailure(result)
            : Ok(result.Value);
    }
}
