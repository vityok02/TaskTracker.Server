using Application.Modules.Users;
using Application.Modules.Users.Identity.RegisterUser;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>();

        CreateMap<RegisterRequest, User>();
    }
}
