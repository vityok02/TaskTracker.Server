using Api.Controllers.Identity.Requests;
using Application.Modules.Identity.Login;
using Application.Modules.Identity.Register;
using AutoMapper;

namespace Api.Mapping;

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>();
        CreateMap<LoginRequest, LoginCommand>();
    }
}
