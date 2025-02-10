using Application.Modules.Identity;
using Domain.Entities;

namespace Application.Abstract.Interfaces;

public interface IJwtProvider
{
    TokenResponse Generate(User user);
}
