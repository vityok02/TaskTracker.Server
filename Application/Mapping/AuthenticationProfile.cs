using Application.Modules.Authentication;
using AutoMapper;
using Domain.Models;

namespace Application.Mapping;

public class AuthenticationProfile : Profile
{
    public AuthenticationProfile()
    {
        CreateMap<TokenModel, TokenDto>();
    }
}
