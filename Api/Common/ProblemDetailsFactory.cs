using Domain.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Api.Common;

public static class ProblemDetailsFactory
{
    public static ProblemDetails CreateProblemDetails(
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

    public static ProblemDetails CreateProblemDetails(
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
