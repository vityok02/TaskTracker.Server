using Domain.Shared;

namespace Domain.Errors;

public static class StateErrors
{
    public static Error NotFound
        => new("State.NotFound", "State not found");
}
