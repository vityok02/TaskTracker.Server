﻿using Application.Abstract.Interfaces.Base;
using Domain.Entities;

namespace Application.Abstract.Interfaces.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email);

    Task<User?> GetUserByEmailOrNameAsync(string email, string username);

    Task<User?> GetByNameAsync(string userName);
}
