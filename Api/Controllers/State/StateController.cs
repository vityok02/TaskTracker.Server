using Api.Controllers.Abstract;
using Api.Controllers.State.Responses;
using Api.Filters;
using Application.Modules.States;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.State;

[ProjectMember]
[Route("projects/{projectId:guid}/states")]
public class StateController : BaseController
{
    public StateController(
        ISender sender,
        IMapper mapper)
        : base(sender, mapper)
    {
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<StateResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        [FromRoute] Guid projectId)
    {
        var query = new GetAllStatesQuery(projectId);

        var result = await Sender
            .Send(query);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<IEnumerable<StateResponse>>(result.Value));
    }
}
