using Application.Modules.Templates;
using AutoMapper;
using Domain.Entities.Templates;

namespace Application.Mapping;

public class TemplateProfile : Profile
{
    public TemplateProfile()
    {
        CreateMap<TemplateEntity, TemplateDto>();
    }
}
