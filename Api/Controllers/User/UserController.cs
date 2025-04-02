using Api.Controllers.Abstract;
using Api.Controllers.User.Responses;
using Api.Extensions;
using Application.Abstract.Interfaces;
using Application.Modules.Users;
using Application.Modules.Users.GetAllUsers;
using Application.Modules.Users.GetUserById;
using Application.Modules.Users.SearchUserQuery;
using Application.Modules.Users.SetAvatar;
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
    private readonly IBlobService _blobService;

    public UserController(
        ISender sender,
        IMapper mapper,
        IBlobService blobService)
        : base(sender, mapper)
    {
        _blobService = blobService;
    }

    [HttpPost("avatar")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UploadAvatar(
        IFormFile file,
        CancellationToken cancellationToken)
    {
        if (file is null)
        {
            return BadRequest();
        }
        
        await using var content = file
            .OpenReadStream();
        
        var command = new SetAvatarCommand(
            file.FileName,
            content,
            User.GetUserId());

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(new { fileUrl = result.Value });
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
