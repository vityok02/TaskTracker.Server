using Application.Modules.Comments;
using AutoMapper;
using Domain.Entities;
using Domain.Models;

namespace Application.Mapping;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CommentEntity, CommentDto>();

        CreateMap<CommentModel, CommentDto>();
    }
}
