using Api.Common;
using Application.Abstract.Interfaces.Repositories;
using Domain.Errors;
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
        var contextUser = context.HttpContext.User;

        if (!contextUser.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedObjectResult(
                ProblemDetailsFactory
                    .CreateProblemDetails(
                        "Unauthorized",
                        StatusCodes.Status401Unauthorized,
                        UserErrors.Unauthorized));

            return;
        }

        var userId = GetUserId(context.HttpContext);

        var userReposiotry = context.HttpContext.RequestServices
            .GetRequiredService<IUserRepository>();

        var user = await userReposiotry
            .GetByIdAsync(userId);

        if (user is null)
        {
            context.Result = new NotFoundObjectResult(
                ProblemDetailsFactory
                    .CreateProblemDetails(
                        "Not Found",
                        StatusCodes.Status404NotFound,
                        UserErrors.NotFound));

            return;
        }

        var projectId = GetProjectId(context.HttpContext);

        var memberRepository = context.HttpContext.RequestServices
            .GetRequiredService<IProjectMemberRepository>();

        var member = await memberRepository
            .GetExtendedAsync(userId, projectId);

        if (member is null)
        {
            context.Result = new NotFoundObjectResult(
                ProblemDetailsFactory
                    .CreateProblemDetails(
                        "Not Found",
                        StatusCodes.Status404NotFound,
                        ProjectMemberErrors.NotFound));

            return;
        }

        if (_role is null)
        {
            return;
        }

        if (member.RoleName != _role)
        {
            context.Result = new ForbiddenObjectResult(
                ProblemDetailsFactory
                    .CreateProblemDetails(
                        "Forbidden",
                        StatusCodes.Status403Forbidden,
                        UserErrors.Forbidden));
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