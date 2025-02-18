using Api.Controllers.Identity.Requests;
using Api.Controllers.Identity.Responses;
using Application.Modules.Identity;
using Application.Modules.Identity.ChangePassword;
using Application.Modules.Identity.Login;
using Application.Modules.Identity.Register;
using Application.Modules.Identity.ResetPassword;
using Application.Modules.Identity.SetPassword;
using AutoMapper;

namespace Api.Mapping;

public class IdentityProfile : Profile
{
    public IdentityProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>();

        CreateMap<LoginRequest, LoginCommand>();

        CreateMap<TokenDto, TokenResponse>();

        CreateMap<ResetPasswordRequest, ResetPasswordCommand>();

        CreateMap<ResetPasswordDto, ResetPasswordResponse>();

        CreateMap<SetPasswordRequest, SetPasswordCommand>();

        CreateMap<ChangePasswordRequest, ChangePasswordCommand>();
    }
}
