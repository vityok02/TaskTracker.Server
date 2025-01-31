using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Base;

public abstract class BaseController : Controller
{
    protected readonly ISender Sender;
    protected readonly LinkGenerator LinkGenerator;

    protected BaseController(ISender sender, LinkGenerator linkGenerator)
    {
        Sender = sender;
        LinkGenerator = linkGenerator;
    }
}
