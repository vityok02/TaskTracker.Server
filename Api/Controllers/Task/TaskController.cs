using Api.Controllers.Abstract;
using Api.Controllers.Task.Requests;
using Api.Controllers.Task.Responses;
using Api.Extensions;
using Api.Filters;
using Application.Modules.Tasks.AddTag;
using Application.Modules.Tasks.CreateTask;
using Application.Modules.Tasks.DeleteTask;
using Application.Modules.Tasks.GetAllTasks;
using Application.Modules.Tasks.GetTaskById;
using Application.Modules.Tasks.RemoveTag;
using Application.Modules.Tasks.ReorderTasks;
using Application.Modules.Tasks.UpdateTask;
using Application.Modules.Tasks.UpdateTaskState;
using AutoMapper;
using Domain.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Task;

[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
[Route("projects/{projectId:guid}/tasks")]
public class TaskController : BaseController
{
    private const string GetByIdAction = "GetTaskById";

    public TaskController(
        ISender sender,
        IMapper mapper)
        : base(sender, mapper)
    {
    }

    [ProjectMember(Roles.Contributor)]
    [HttpPost]
    [ProducesResponseType<TaskResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid projectId,
        [FromBody] TaskRequest taskRequest,
        CancellationToken token)
    {
        var command = new CreateTaskCommand(
            taskRequest.Name,
            taskRequest.Description,
            taskRequest.StartDate,
            User.GetUserId(),
            projectId,
            taskRequest.StateId);

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : CreatedAtAction(
                GetByIdAction,
                new { 
                    projectId,
                    taskId = result.Value.Id
                },
                Mapper.Map<TaskResponse>(result.Value));
    }

    [ProjectMember]
    [HttpGet("{taskId:guid}")]
    [ActionName(GetByIdAction)]
    [ProducesResponseType<TaskResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid taskId,
        CancellationToken token)
    {
        var query = new GetTaskByIdQuery(projectId, taskId);

        var result = await Sender
            .Send(query, token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<TaskResponse>(result.Value));
    }

    [ProjectMember]
    [HttpGet]
    [ProducesResponseType<IEnumerable<TaskResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllAsync(
        [FromRoute] Guid projectId,
        [FromQuery] string? searchTerm,
        CancellationToken token)
    {
        var query = new GetAllTasksQuery(projectId, searchTerm);

        var result = await Sender
            .Send(query, token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<IEnumerable<TaskResponse>>(result.Value));
    }

    [ProjectMember(Roles.Contributor)]
    [HttpPut("{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid taskId,
        [FromBody] TaskRequest taskRequest,
        CancellationToken token)
    {
        var command = new UpdateTaskCommand(
            taskId,
            taskRequest.Name,
            taskRequest.Description,
            taskRequest.StartDate,
            taskRequest.EndDate,
            User.GetUserId(),
            projectId,
            taskRequest.StateId);

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }

    [ProjectMember(Roles.Contributor)]
    [HttpPatch("{taskId:guid}/state")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateStateAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid taskId,
        [FromBody] UpdateTaskStateRequest request)
    {
        var command = new UpdateTaskStateCommand(
            taskId,
            request.StateId,
            request.BeforeTaskId,
            projectId,
            User.GetUserId());

        var result = await Sender
            .Send(command);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }

    [ProjectMember(Roles.Contributor)]
    [HttpPatch("{taskId:guid}/order")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReorderAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid taskId,
        [FromBody] ReorderTasksRequest request)
    {
        var command = new ReorderTasksCommand(
            taskId,
            request.BeforeTaskId,
            projectId,
            User.GetUserId());

        var result = await Sender
            .Send(command);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }

    [ProjectMember(Roles.Contributor)]
    [HttpDelete("{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid taskId,
        CancellationToken token)
    {
        var command = new DeleteTaskCommand(taskId, projectId);

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }

    [ProjectMember(Roles.Contributor)]
    [HttpPut("{taskId:guid}/tags/{tagId:guid}")]
    [ProducesResponseType<TaskResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<TaskResponse>(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddTagAsync(
        [FromRoute] Guid taskId,
        [FromRoute] Guid tagId,
        CancellationToken cancellationToken)
    {
        var command = new AddTagCommand(
            taskId,
            tagId,
            User.GetUserId());

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<TaskResponse>(result.Value));
    }

    [ProjectMember(Roles.Contributor)]
    [HttpDelete("{taskId:guid}/tags/{tagId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTagAsync(
        [FromRoute] Guid taskId,
        [FromRoute] Guid tagId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTagCommand(
            taskId,
            tagId,
            User.GetUserId());

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }
}
