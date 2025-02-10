using Api.Controllers.Base;
using Api.Controllers.Identity.Requests;
using Application.Modules.Identity;
using Application.Modules.Identity.Login;
using Application.Modules.Identity.Register;
using Application.Modules.Users.Identity.RegisterUser;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Identity;

[Route("users")]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
public class IdentityController : BaseController
{
    private readonly IMapper _mapper;

    public IdentityController(
        ISender sender,
        LinkGenerator linkGenerator,
        IMapper mapper)
        : base(sender, linkGenerator)
    {
        _mapper = mapper;
    }

    [HttpPost("register")]
    [ProducesResponseType<RegisterResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<RegisterResponse>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest registerRequest,
        CancellationToken token)
    {
        var registerCommand = _mapper
            .Map<RegisterCommand>(registerRequest);

        var result = await Sender
            .Send(registerCommand, token);

        if (result.IsFailure)
        {
            return HandlerFailure(result);
        }

        var uri = LinkGenerator.GetPathByName(
            HttpContext,
            "GetUser",
            new { result.Value.Id });

        return Created(uri, result.Value);
    }

    [HttpPost("login")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest loginRequest,
        CancellationToken token)
    {
        var loginCommand = _mapper
            .Map<LoginCommand>(loginRequest);

        var result = await Sender
            .Send(loginCommand, token);

        return result.IsFailure
            ? HandlerFailure(result)
            : Ok(result.Value);
    }
}
