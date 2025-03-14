using Api.Controllers.Abstract;
using Api.Controllers.Comment.Requests;
using Api.Controllers.Comment.Responses;
using Api.Extensions;
using Api.Filters;
using Application.Modules.Comments.CreateComment;
using Application.Modules.Comments.DeleteComment;
using Application.Modules.Comments.GetAllComments;
using Application.Modules.Comments.GetCommentById;
using Application.Modules.Comments.UpdateComment;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Comment;

[ProjectMember]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status401Unauthorized)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status403Forbidden)]
[ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
[Route("/projects/{projectId:guid}/tasks/{taskId:guid}/comments")]
public class CommentController : BaseController
{
    private const string GetByIdAction = "GetCommentById";

    public CommentController(
        ISender sender,
        IMapper mapper)
        : base(sender, mapper)
    {
    }

    [HttpPost]
    [ProducesResponseType<CommentResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid taskId,
        [FromBody] CommentRequest commentRequest,
        CancellationToken cancellationToken)
    {
        var command = new CreateCommentCommand(
            commentRequest.Comment,
            taskId,
            User.GetUserId());

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : CreatedAtAction(
                GetByIdAction,
                new
                {
                    projectId,
                    taskId,
                    commentId = result.Value.Id
                },
                Mapper.Map<CommentResponse>(result.Value));
    }

    [HttpGet("{commentId:guid}")]
    [ActionName(GetByIdAction)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid taskId,
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var command = new GetCommentByIdQuery(commentId);

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<CommentResponse>(result.Value));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid taskId,
        CancellationToken cancellationToken)
    {
        var command = new GetAllCommentsQuery(taskId);

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : Ok(Mapper.Map<IEnumerable<CommentResponse>>(result.Value));
    }

    [HttpPut("{commentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid taskId,
        [FromRoute] Guid commentId,
        [FromBody] CommentRequest commentRequest,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCommentCommand(
            commentId,
            commentRequest.Comment,
            User.GetUserId());

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }

    [HttpDelete("{commentId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid projectId,
        [FromRoute] Guid taskId,
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCommentCommand(
            commentId,
            User.GetUserId());

        var result = await Sender
            .Send(command, cancellationToken);

        return result.IsFailure
            ? HandleFailure(result)
            : NoContent();
    }
}
