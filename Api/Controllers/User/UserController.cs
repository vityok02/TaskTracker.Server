using Api.Controllers.Abstract;
using Api.Controllers.User.Responses;
using Application.Modules.Users;
using Application.Modules.Users.GetAllUsers;
using Application.Modules.Users.GetUserById;
using Application.Modules.Users.SearchUserQuery;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.User;

[Authorize]
[Route("users")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public sealed class UserController : BaseController
{
    public UserController(
        ISender sender,
        IMapper mapper)
        : base(sender, mapper)
    {
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id,
        CancellationToken token)
    {
        var result = await Sender
            .Send(new GetUserByIdQuery(id), token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<UserDto>(result.Value));
    }

    [HttpGet("search")]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<UserResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSeveralByNameAsync(
        [FromQuery] string username,
        CancellationToken token)
    {
        var result = await Sender
            .Send(new SearchUsersQuery(username), token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<IEnumerable<UserDto>>(result.Value));
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<UserResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] string? username,
        CancellationToken token)
    {
        var result = username is null
            ? await Sender.Send(new GetAllUsersQuery(), token)
            : await Sender.Send(new SearchUsersQuery(username), token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<IEnumerable<UserDto>>(result.Value));
    }
}
