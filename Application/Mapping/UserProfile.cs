using Api.Users.Dtos;
using Application.Users.GetUser;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>();

        CreateMap<RegisterRequest, User>();
    }
}
