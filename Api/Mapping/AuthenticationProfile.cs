using Api.Controllers.Authentication.Requests;
using Api.Controllers.Authentication.Responses;
using Application.Modules.Authentication;
using Application.Modules.Authentication.Login;
using Application.Modules.Authentication.Register;
using AutoMapper;
using Domain.Models;

namespace Api.Mapping;

public class AuthenticationProfile : Profile
{
    public AuthenticationProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>();
        CreateMap<LoginRequest, LoginCommand>();
        CreateMap<RegisterDto, RegisterResponse>();
        CreateMap<TokenDto, TokenResponse>();
    }
}
