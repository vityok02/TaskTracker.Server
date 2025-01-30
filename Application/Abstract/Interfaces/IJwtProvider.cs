using Domain.Entities;

namespace Application.Abstract.Interfaces;

public interface IJwtProvider
{
    string Generate(User user);
}
