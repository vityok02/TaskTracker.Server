using Api.Controllers.Tag.Responses;
using Application.Modules.Tags;
using AutoMapper;

namespace Api.Mapping;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<TagDto, TagResponse>();
    }
}
