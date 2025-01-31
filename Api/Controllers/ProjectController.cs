using Api.Controllers.Base;
using Api.Extensions;
using Api.Filters;
using Application.Modules.Projects.CreateProject;
using Application.Modules.Projects.GetProjectById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Authorize]
[Route("projects")]
public class ProjectController : BaseController
{
    private Guid UserId => User.GetUserIdFromClaims();

    public ProjectController(
        ISender sender,
        LinkGenerator linkGenerator)
        : base(sender, linkGenerator)
    {
    }

    [HttpPost]
    [EndpointName(nameof(CreateProject))]
    public async Task<IActionResult> CreateProject(
        [FromBody] ProjectRequest projectRequest)
    {
        var command = new CreateProjectCommand(
            UserId,
            projectRequest);

        var result = await Sender
            .Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var uri = LinkGenerator.GetUriByAction(
            HttpContext,
            nameof(GetProject),
            values: new { result.Value.Id });

        return Created(uri, result.Value);
    }

    [HttpGet("{projectId:guid}")]
    [EndpointName(nameof(GetProject))]
    [ProjectMember]
    public async Task<IActionResult> GetProject(
        [FromRoute] Guid projectId)
    {
        var query = new GetProjectQuery(UserId, projectId);

        var result = await Sender
            .Send(query);

        // TODO: return specific status code.
        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}
