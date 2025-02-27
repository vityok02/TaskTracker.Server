using Api.Controllers.Abstract;
using Api.Controllers.Task.Requests;
using Api.Controllers.Task.Responses;
using Api.Extensions;
using Api.Filters;
using Application.Modules.Tasks.CreateTask;
using Application.Modules.Tasks.DeleteTask;
using Application.Modules.Tasks.GetAllTasks;
using Application.Modules.Tasks.GetTaskById;
using Application.Modules.Tasks.UpdateTask;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Task;

[ProjectMember]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
[Route("projects/{projectId:guid}/tasks")]
public class TaskController : BaseController
{
    public TaskController(
        ISender sender,
        IMapper mapper)
        : base(sender, mapper)
    {
    }

    [HttpPost]
    [ProducesResponseType<TaskResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTask(
        [FromRoute] Guid projectId,
        [FromBody] CreateTaskRequest taskRequest,
        CancellationToken token)
    {
        var command = new CreateTaskCommand(
            taskRequest.Name,
            taskRequest.Description,
            User.GetUserId(),
            projectId,
            taskRequest.StateId);

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : CreatedAtAction(
                nameof(GetTask),
                new { 
                    projectId,
                    taskId = result.Value.Id
                },
                Mapper.Map<TaskResponse>(result.Value));
    }

    [HttpGet("{taskId:guid}", Name = nameof(GetTask))]
    [ProducesResponseType<TaskResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTask(
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

    [HttpGet]
    [ProducesResponseType<IEnumerable<TaskResponse>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTasks(
        [FromRoute] Guid projectId,
        CancellationToken token)
    {
        var query = new GetAllTasksQuery(projectId);

        var result = await Sender
            .Send(query, token);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<IEnumerable<TaskResponse>>(result.Value));
    }

    [HttpPut("{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateTask(
        [FromRoute] Guid projectId,
        [FromRoute] Guid taskId,
        [FromBody] UpdateTaskRequest taskRequest,
        CancellationToken token)
    {
        var command = new UpdateTaskCommand(
            taskId,
            projectId,
            User.GetUserId(),
            taskRequest.StateId,
            taskRequest.Name,
            taskRequest.Description
            );

        var result = await Sender
            .Send(command, token);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }

    [HttpDelete("{taskId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteTask(
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
}
