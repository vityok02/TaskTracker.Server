using Application.Modules.Tags;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class TagProfile : Profile
{
    public TagProfile()
    {
        CreateMap<TagEntity, TagDto>();
    }
}
