using Application.Abstract.Interfaces.Repositories;
using Application.Abstract.Messaging;
using AutoMapper;
using Domain.Shared;

namespace Application.Modules.Templates.GetAllTemplates;

internal sealed class GetAllTemplatesQueryHandler
    : IQueryHandler<GetAllTemplatesQuery, IEnumerable<TemplateDto>>
{
    private readonly ITemplateRepository _templateRepository;
    private readonly IMapper _mapper;

    public GetAllTemplatesQueryHandler(
        ITemplateRepository templateRepository,
        IMapper mapper)
    {
        _templateRepository = templateRepository;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<TemplateDto>>> Handle(
        GetAllTemplatesQuery query,
        CancellationToken cancellationToken)
    {
        var templates = await _templateRepository
            .GetAllAsync();

        return Result<IEnumerable<TemplateDto>>.Success(
            _mapper.Map<IEnumerable<TemplateDto>>(templates.OrderBy(t => t.SortOrder)));
    }
}
