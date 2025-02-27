using AutoMapper;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Abstract;

public abstract class BaseController : Controller
{
    protected readonly ISender Sender;
    protected readonly IMapper Mapper;

    protected BaseController(
        ISender sender,
        IMapper mapper)
    {
        Sender = sender;
        Mapper = mapper;
    }

    protected IActionResult HandleFailure(Result result)
    {
        return result.Error.Code switch
        {
            ErrorTypes.ValidationError when result is IValidationResult validationResult => BadRequest(
                CreateProblemDetails(
                    "Validation Error", StatusCodes.Status400BadRequest,
                    result.Error,
                    validationResult.Errors)),

            ErrorTypes.InvalidToken => BadRequest(
                CreateProblemDetails(
                    "Invalid Token",
                    StatusCodes.Status400BadRequest,
                    result.Error)),

            ErrorTypes.Unauthorized => Unauthorized(
                CreateProblemDetails(
                    "Unauthorized",
                    StatusCodes.Status401Unauthorized,
                    result.Error)),

            ErrorTypes.InvalidCredentials => Unauthorized(
                CreateProblemDetails(
                    "Invalid Credentials",
                    StatusCodes.Status401Unauthorized,
                    result.Error)),

            var code when code.EndsWith(ErrorTypes.NotFound) => NotFound(
                CreateProblemDetails(
                    "Not Found",
                    StatusCodes.Status404NotFound,
                    result.Error)),

            var code when code.EndsWith(ErrorTypes.Conflict) => Conflict(
                CreateProblemDetails(
                    "Conflict",
                    StatusCodes.Status409Conflict,
                    result.Error)),

            _ => BadRequest(
                CreateProblemDetails(
                    "Bad Request",
                    StatusCodes.Status400BadRequest,
                    result.Error))
        };
    }

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };

    private static ProblemDetails CreateProblemDetails(
    string title,
    int status,
    Error error) =>
    new()
    {
        Title = title,
        Type = error.Code,
        Detail = error.Message,
        Status = status,
        Extensions = { { "resouceType", error.Code.Split(',')[0] } }
    };
}