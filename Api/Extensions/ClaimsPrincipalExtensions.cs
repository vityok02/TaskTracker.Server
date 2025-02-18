using System.Security.Claims;

namespace Api.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        return Guid
            .Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }
}
