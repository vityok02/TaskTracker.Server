using Application.Abstract.Interfaces.Repositories;
using Dapper;
using Domain.Entities.Templates;
using Persistence.Abstractions;
using Persistence.Repositories.Base;

namespace Persistence.Repositories;

public class TemplateRepository
    : BaseRepository<TemplateEntity, Guid>, ITemplateRepository
{
    public TemplateRepository(ISqlConnectionFactory connectionFactory)
        : base(connectionFactory)
    {
    }

    public async Task<IEnumerable<TemplateStateEntity>> GetStatesAsync(Guid templateId)
    {
        using var connection = ConnectionFactory.Create();

        var query = $@"SELECT * FROM [TemplateState]
            WHERE TemplateId = @TemplateId";

        return await connection
            .QueryAsync<TemplateStateEntity>(
                query,
                new { TemplateId = templateId });
    }

    public async Task<IEnumerable<TemplateTagEntity>> GetTagsAsync(Guid templateId)
    {
        using var connection = ConnectionFactory.Create();

        var query = $@"SELECT * FROM [TemplateTag]
            WHERE TemplateId = @TemplateId";

        return await connection
            .QueryAsync<TemplateTagEntity>(
                query,
                new { TemplateId = templateId });
    }
}
