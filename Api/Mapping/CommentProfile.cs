using Api.Controllers.Comment.Responses;
using Application.Modules.Comments;
using AutoMapper;

namespace Api.Mapping;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<CommentDto, CommentResponse>();
    }
}
