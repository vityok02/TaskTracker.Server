using Application.Modules.Comments;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CommentModel, CommentDto>();
    }
}
