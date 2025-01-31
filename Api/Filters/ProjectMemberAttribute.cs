using Application.Abstract.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Api.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class ProjectMemberAttribute
    : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _role = null!;

    public ProjectMemberAttribute()
    { }

    public ProjectMemberAttribute(string role)
    {
        _role = role;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var memberRepository = context.HttpContext.RequestServices
            .GetRequiredService<IProjectMemberRepository>();

        var userId = GetUserId(context.HttpContext);
        var projectId = GetProjectId(context.HttpContext);

        var member = await memberRepository
            .GetAsync(userId, projectId);

        if (member is null)
        {
            context.Result = new ForbidResult();
            return;
        }

        if (_role is null)
        {
            return;
        }

        var role = await memberRepository
            .GetMemberRole(userId, projectId);

        if (role?.Name != _role)
        {
            context.Result = new ForbidResult();
        }
    }

    private static Guid GetUserId(HttpContext context)
    {
        var userIdFromClaims = context.User
            .FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(userIdFromClaims, out var userId)
            ? userId
            : Guid.Empty;
    }

    private static Guid GetProjectId(HttpContext context)
    {
        var projectIdFromRoute = context.Request
            .RouteValues["projectId"]?.ToString();

        return Guid.TryParse(projectIdFromRoute, out var projectId)
            ? projectId
            : Guid.Empty;
    }
}