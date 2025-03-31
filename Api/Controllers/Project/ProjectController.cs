using Api.Controllers.Abstract;
using Api.Controllers.Project.Requests;
using Api.Controllers.Project.Responses;
using Api.Extensions;
using Api.Filters;
using Application.Modules.Projects.CreateProject;
using Application.Modules.Projects.DeleteProject;
using Application.Modules.Projects.GetPagedProjects;
using Application.Modules.Projects.GetProjectById;
using Application.Modules.Projects.UpdateProject;
using AutoMapper;
using Domain.Abstract;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Project;

[Authorize]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
[Route("/projects")]
public class ProjectController : BaseController
{
    public const string GetByIdAction = "GetProjectById";
    public ProjectController(
        ISender sender,
        IMapper mapper)
        : base(sender, mapper)
    {
    }

    [HttpPost]
    [ProducesResponseType<ProjectResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateAsync(
        [FromBody] CreateProjectRequest projectRequest,
        CancellationToken token)
    {
        var command = new CreateProjectCommand(
            User.GetUserId(),
            projectRequest.Name,
            projectRequest.Description);

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : CreatedAtAction(
                GetByIdAction,
                new { projectId = result.Value.Id },
                Mapper.Map<ProjectResponse>(result.Value));
    }

    [HttpGet("{projectId:guid}")]
    [ActionName(GetByIdAction)]
    [ProducesResponseType<ProjectResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid projectId,
        CancellationToken token)
    {
        var query = new GetProjectQuery(
            projectId);

        var result = await Sender
            .Send(query, token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<ProjectResponse>(result.Value));
    }

    [HttpGet]
    [ProducesResponseType<PagedList<ProjectResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? searchTerm,
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        CancellationToken token)
    {
        var query = new GetPagedProjectsQuery(
            page,
            pageSize,
            searchTerm,
            sortColumn,
            sortOrder,
            User.GetUserId());

        var result = await Sender
            .Send(query, token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<PagedList<ProjectResponse>>(result.Value));
    }

    [ProjectMember(Roles.Admin)]
    [HttpPut("{projectId:guid}")]
    [ProducesResponseType<ProjectResponse>(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProject(
        [FromRoute] Guid projectId,
        [FromBody] UpdateProjectRequest projectRequest,
        CancellationToken token)
    {
        var command = new UpdateProjectCommand(
            User.GetUserId(),
            projectId,
            projectRequest.Name,
            projectRequest.Description);

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }

    [ProjectMember(Roles.Admin)]
    [HttpDelete("{projectId:guid}")]
    [ProducesResponseType<ProjectResponse>(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProject(
        [FromRoute] Guid projectId,
        CancellationToken token)
    {
        var command = new DeleteProjectCommand(projectId);

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }
}
