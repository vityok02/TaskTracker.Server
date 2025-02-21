using Api.Controllers.Role.Responses;
using Application.Modules.Roles;
using AutoMapper;

namespace Api.Mapping;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleDto, RoleResponse>();
    }
}
