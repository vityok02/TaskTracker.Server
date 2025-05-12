using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Abstract;
using Domain.Entities;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

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
}
