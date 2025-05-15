using Api.Controllers.Template;
using Application.Modules.Templates;
using AutoMapper;

namespace Api.Mapping;

public class TemplateProfile : Profile
{
    public TemplateProfile()
    {
        CreateMap<TemplateDto, TemplateResponse>();
    }
}
