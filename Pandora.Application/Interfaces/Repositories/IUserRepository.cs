﻿using Pandora.Core.Domain.Entities;
using Pandora.Core.Domain.Interfaces;

namespace Pandora.Application.Interfaces.Repositories;

public interface IUserRepository : IAsyncRepository<User, Guid>, IRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
}
