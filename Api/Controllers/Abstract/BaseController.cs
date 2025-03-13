using AutoMapper;
using Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Api.Common.ProblemDetailsFactory;

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
        if (Enum.TryParse<ErrorType>(result.Error.Code, out var errorType))
        {
            return errorType switch
            {
                ErrorType.ValidationError when result is IValidationResult validationResult => BadRequest(
                    CreateProblemDetails(
                        "Validation Error",
                        StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),

                ErrorType.InvalidToken => BadRequest(
                    CreateProblemDetails(
                        "Invalid Token",
                        StatusCodes.Status400BadRequest,
                        result.Error)),

                ErrorType.Unauthorized => Unauthorized(
                    CreateProblemDetails(
                        "Unauthorized",
                        StatusCodes.Status401Unauthorized,
                        result.Error)),

                ErrorType.InvalidCredentials => Unauthorized(
                    CreateProblemDetails(
                        "Invalid Credentials",
                        StatusCodes.Status401Unauthorized,
                        result.Error)),

                ErrorType.NotFound => NotFound(
                    CreateProblemDetails(
                        "Not Found",
                        StatusCodes.Status404NotFound,
                        result.Error)),

                ErrorType.AlreadyExists => Conflict(
                    CreateProblemDetails(
                        "Conflict",
                        StatusCodes.Status409Conflict,
                        result.Error)),

                ErrorType.InvalidOperation => BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Error)),

                _ => BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Error))
            };
        }

        if (result.Error.Code.EndsWith(ErrorType.NotFound.ToString()))
        {
            return NotFound(CreateProblemDetails("Not Found", StatusCodes.Status404NotFound, result.Error));
        }

        if (result.Error.Code.EndsWith(ErrorType.AlreadyExists.ToString()))
        {
            return Conflict(CreateProblemDetails("Conflict", StatusCodes.Status409Conflict, result.Error));
        }

        return BadRequest(CreateProblemDetails("Bad Request", StatusCodes.Status400BadRequest, result.Error));
    }
}
