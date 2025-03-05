﻿using Microsoft.AspNetCore.Mvc;

namespace Api.Common;

public class ForbiddenObjectResult : ObjectResult
{
    public ForbiddenObjectResult(object? value)
        : base(value)
    {
        StatusCode = StatusCodes.Status403Forbidden;
    }
}
