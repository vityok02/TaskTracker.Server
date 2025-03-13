using Api.Controllers.Authentication.Requests;
using Api.Controllers.Authentication.Responses;
using Application.Modules.Authentication;
using Application.Modules.Authentication.ChangePassword;
using Application.Modules.Authentication.Login;
using Application.Modules.Authentication.Register;
using Application.Modules.Authentication.ResetPassword;
using Application.Modules.Authentication.SetPassword;
using AutoMapper;

namespace Api.Mapping;

public class AuthenticationProfile : Profile
{
    public AuthenticationProfile()
    {
        CreateMap<ResetPasswordDto, ResetPasswordResponse>();
        CreateMap<RegisterDto, RegisterResponse>();
        CreateMap<TokenDto, TokenResponse>();
        CreateMap<RegisterRequest, RegisterCommand>();
        CreateMap<LoginRequest, LoginCommand>();
        CreateMap<ResetPasswordRequest, ResetPasswordCommand>();
        CreateMap<SetPasswordRequest, SetPasswordCommand>();
        CreateMap<ChangePasswordRequest, ChangePasswordCommand>();
    }
}
