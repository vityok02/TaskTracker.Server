using Application.Modules.Identity;
using Domain.Entities;

namespace Application.Abstract.Interfaces;

public interface IJwtProvider
{
    TokenDto Generate(User user);
}
