using Domain.Shared;

namespace Domain.Errors;

public static class CommentErrors
{
    public static Error NotFound
        => new("Comment.NotFound", "Comment not found.");

    public static Error Forbidden
        => new("Comment.Forbidden", "You do not have permission to modify this comment.");
}
