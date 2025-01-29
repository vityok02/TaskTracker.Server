using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Base;

public abstract class ApiController : Controller
{
    protected readonly ISender Sender;
    protected readonly LinkGenerator LinkGenerator;

    protected ApiController(ISender sender, LinkGenerator linkGenerator)
    {
        Sender = sender;
        LinkGenerator = linkGenerator;
    }
}
