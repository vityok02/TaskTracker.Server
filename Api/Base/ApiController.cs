using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Base;

public abstract class ApiController : Controller
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }
}
