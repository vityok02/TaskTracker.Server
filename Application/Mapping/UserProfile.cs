﻿using Application.Dtos;
using Application.Modules.Users;
using AutoMapper;
using Domain.Entities;
using Domain.Models.Common;

namespace Application.Mapping;

public sealed class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserEntity, UserDto>();

        CreateMap<UserInfoModel, UserInfoDto>();
    }
}
