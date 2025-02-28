using Application.Abstract.Interfaces.Base;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface IUserRepository : IRepository<UserEntity, Guid>
{
    Task<UserEntity?> GetByEmailAsync(string email);

    Task<UserEntity?> GetUserByEmailOrNameAsync(string email, string username);

    Task<UserEntity?> GetByNameAsync(string userName);
}
