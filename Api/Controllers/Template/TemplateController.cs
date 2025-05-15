using Api.Controllers.Abstract;
using Application.Modules.Templates.GetAllTemplates;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Template;

public class TemplateController : BaseController
{
    public TemplateController(
        ISender sender,
        IMapper mapper)
        : base(sender, mapper)
    {
    }

    [HttpGet("/templates")]
    [ProducesResponseType<TemplateResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        CancellationToken token)
    {
        var command = new GetAllTemplatesQuery();

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<IEnumerable<TemplateResponse>>(result.Value));
    }
}
