using Api.Controllers.Comment.Responses;
using Api.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace Api.Services;

public class CommentsHubService : ICommentsHubService
{
    private readonly IHubContext<CommentsHub, ICommentsHub> _hubContext;

    public CommentsHubService(
        IHubContext<CommentsHub, ICommentsHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task SendCommentCreated(CommentResponse comment)
    {
        return _hubContext.Clients.All
            .ReceiveCommentCreated(comment);
    }

    public Task SendCommentDeleted(Guid commentId)
    {
        return _hubContext.Clients.All
            .ReceiveCommentDeleted(commentId);
    }

    public Task SendCommentUpdated(CommentResponse comment)
    {
        return _hubContext.Clients.All
            .ReceiveCommentUpdated(comment);
    }
}
