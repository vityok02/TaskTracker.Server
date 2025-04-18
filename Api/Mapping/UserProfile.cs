﻿using Api.Common;
using Api.Controllers.User.Responses;
using Application.Common.Dtos;
using Application.Modules.Users;
using AutoMapper;

namespace Api.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, UserResponse>();

        CreateMap<UserInfoDto, UserInfoResponse>();
    }
}
