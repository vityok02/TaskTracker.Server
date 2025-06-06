﻿using Api.Controllers.Abstract;
using Api.Controllers.Authentication.Requests;
using Api.Controllers.Authentication.Responses;
using Api.Extensions;
using Application.Modules.Authentication.ChangePassword;
using Application.Modules.Authentication.Login;
using Application.Modules.Authentication.Register;
using Application.Modules.Authentication.ResetPassword;
using Application.Modules.Authentication.SetPassword;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Authentication;

[ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
public class AuthenticationController : BaseController
{
    private readonly LinkGenerator _linkGenerator;

    public AuthenticationController(
        ISender sender,
        IMapper mapper,
        LinkGenerator linkGenerator)
        : base(sender, mapper)
    {
        _linkGenerator = linkGenerator;
    }

    [HttpPost("register")]
    [ProducesResponseType<RegisterResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterAsync(
        [FromBody] RegisterRequest registerRequest,
        CancellationToken token)
    {
        var registerCommand = Mapper
            .Map<RegisterCommand>(registerRequest);

        var result = await Sender
            .Send(registerCommand, token);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        var uri = _linkGenerator.GetPathByName(
            HttpContext,
            "GetUser",
            new { result.Value.Id });

        return Created(
            uri,
            Mapper.Map<RegisterResponse>(result.Value));
    }

    [HttpPost("login")]
    [ProducesResponseType<RegisterResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> LoginAsync(
        [FromBody] LoginRequest loginRequest,
        CancellationToken token)
    {
        var loginCommand = Mapper
            .Map<LoginCommand>(loginRequest);

        var result = await Sender
            .Send(loginCommand, token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<TokenResponse>(result.Value));
    }

    [HttpPost("reset-password")]
    [ProducesResponseType<ResetPasswordResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ResetPasswordAsync(
        [FromBody] ResetPasswordRequest resetPasswordRequest)
    {
        var resetPasswordCommand = Mapper
            .Map<ResetPasswordCommand>(resetPasswordRequest);

        var result = await Sender
            .Send(resetPasswordCommand);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<ResetPasswordResponse>(result.Value));
    }

    [HttpPost("set-password")]
    [ProducesResponseType<TokenResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetPasswordAsync(
        [FromBody] SetPasswordRequest setPasswordRequest)
    {
        var setPasswordCommand = Mapper
            .Map<SetPasswordCommand>(setPasswordRequest);

        var result = await Sender
            .Send(setPasswordCommand);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<TokenResponse>(result.Value));
    }

    [Authorize]
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangePasswordAsync(
        [FromBody] ChangePasswordRequest changePasswordRequest)
    {
        var changePasswordCommand = new ChangePasswordCommand(
            User.GetUserId(),
            changePasswordRequest.CurrentPassword,
            changePasswordRequest.NewPassword,
            changePasswordRequest.ConfirmedPassword);

        var result = await Sender
            .Send(changePasswordCommand);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }
}
