using Application.Abstract.Interfaces.Base;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email);

    Task<bool> IsEmailUniqueAsync(string email);

    //Task<User?> GetByUserNameAsync(string userName);
}
