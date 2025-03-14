using Api.Controllers.Abstract;
using Api.Controllers.State.Requests;
using Api.Controllers.State.Responses;
using Api.Extensions;
using Api.Filters;
using Application.Modules.States.CreateState;
using Application.Modules.States.DeleteState;
using Application.Modules.States.GetProjectStates;
using Application.Modules.States.GetStateById;
using Application.Modules.States.UpdateState;
using AutoMapper;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.State;

[ProjectMember]
[Route("projects/{projectId:guid}/states")]
public class StateController : BaseController
{
    private const string GetByIdAction = "GetStateById";

    public StateController(
        ISender sender,
        IMapper mapper)
        : base(sender, mapper)
    {
    }

    [HttpPost]
    [ProducesResponseType<StateResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid projectId,
        [FromBody] CreateStateRequest stateRequest,
        CancellationToken cancellationToken)
    {
        var command = new CreateStateCommand(
            stateRequest.Name,
            stateRequest.Description,
            projectId,
            User.GetUserId());

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : CreatedAtAction(
                GetByIdAction,
                new { projectId, stateId = result.Value.Id },
                Mapper.Map<StateResponse>(result.Value));
    }

    [HttpGet("{stateId:guid}")]
    [ActionName(GetByIdAction)]
    [ProducesResponseType<StateResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid stateId,
        CancellationToken cancellationToken)
    {
        var query = new GetStateByIdQuery(stateId);

        var result = await Sender
            .Send(query, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<StateResponse>(result.Value));
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<StateResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        [FromRoute] Guid projectId,
        CancellationToken cancellationToken)
    {
        var query = new GetProjectStatesQuery(projectId);

        var result = await Sender
            .Send(query, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<IEnumerable<StateResponse>>(result.Value));
    }

    [HttpPut("{stateId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid stateId,
        [FromBody] UpdateStateRequest stateRequest,
        CancellationToken cancellationToken)
    {
        var command = new UpdateStateCommand(
            stateId,
            stateRequest.Name,
            stateRequest.Description,
            stateRequest.Number,
            User.GetUserId());

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }

    [ProjectMember(Roles.Admin)]
    [HttpDelete("{stateId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid stateId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteStateCommand(stateId);

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }
}
