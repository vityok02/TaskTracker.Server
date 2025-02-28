using Application.Modules.Authentication;
using Domain.Entities;
using Domain.Models;

namespace Application.Abstract.Interfaces;

public interface IJwtProvider
{
    TokenModel Generate(UserEntity user);
}
