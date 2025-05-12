using Domain.Shared;

namespace Domain.Errors;

public static class TagErrors
{
    public static Error NotFound
        => new("Tag.NotFound", "Tag not found");
}
