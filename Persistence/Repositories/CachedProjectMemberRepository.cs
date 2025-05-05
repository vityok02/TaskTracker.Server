using Application.Abstract.Interfaces.Repositories;
using Domain.Entities;
using Domain.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Persistence.Repositories;

public class CachedProjectMemberRepository : IProjectMemberRepository
{
    private readonly ProjectMemberRepository _decorated;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<CachedProjectMemberRepository> _logger;

    private static DistributedCacheEntryOptions EntryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
    };

    public CachedProjectMemberRepository(
        ProjectMemberRepository innerRepository,
        IDistributedCache distributedCache,
        ILogger<CachedProjectMemberRepository> logger)
    {
        _decorated = innerRepository;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<ProjectMemberModel> CreateAsync(Guid userId, Guid projectId, Guid roleId)
    {
        return await _decorated.CreateAsync(userId, projectId, roleId);
    }

    public async Task<ProjectMemberEntity?> GetAsync(Guid userId, Guid projectId)
    {
        return await _decorated.GetAsync(userId, projectId);
    }

    public async Task<ProjectMemberModel?> GetExtendedAsync(Guid userId, Guid projectId)
    {
        try
        {
            string key = $"project-member-{userId}-{projectId}";

            string? cachedMember = await _distributedCache
                .GetStringAsync(key);

            ProjectMemberModel? member;

            if (string.IsNullOrEmpty(cachedMember))
            {
                member = await _decorated
                    .GetExtendedAsync(userId, projectId);

                if (member is null)
                {
                    return member;
                }

                await _distributedCache
                    .SetStringAsync(key, JsonSerializer.Serialize(member), EntryOptions);

                return member;
            }

            member = JsonSerializer
                .Deserialize<ProjectMemberModel>(cachedMember);

            return member;
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to connect to redis: {exception}", ex);

            return await _decorated
                .GetExtendedAsync(userId, projectId);
        }
    }

    public async Task<IEnumerable<ProjectMemberModel>> GetAllExtendedAsync(Guid projectId)
    {
        return await _decorated.GetAllExtendedAsync(projectId);
    }

    public async Task UpdateAsync(ProjectMemberEntity projectMember)
    {
        await _decorated.UpdateAsync(projectMember);
    }

    public async Task DeleteAsync(ProjectMemberEntity projectMember)
    {
        await _decorated.DeleteAsync(projectMember);
    }
}
