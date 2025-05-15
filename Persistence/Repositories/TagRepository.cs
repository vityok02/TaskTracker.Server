using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities;
using Persistence.Abstractions;
using Persistence.Repositories.Base;
using Z.Dapper.Plus;

namespace Persistence.Repositories;

public class TagRepository :
    BaseRepository<TagEntity, Guid>, ITagRepository
{
    public TagRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<TagEntity>> GetAllAsync(Guid projectId)
    {
        using var connection = ConnectionFactory.Create();

        var query = $"SELECT * FROM [Tag] WHERE ProjectId = @ProjectId";

        return await connection
            .QueryAsync<TagEntity>(
                query,
                new { ProjectId = projectId });
    }

    public async Task UpdateRangeAsync(IEnumerable<TagEntity> tags)
    {
        using var connection = ConnectionFactory.Create();

        await connection
            .BulkUpdateAsync(tags);
    }
}
