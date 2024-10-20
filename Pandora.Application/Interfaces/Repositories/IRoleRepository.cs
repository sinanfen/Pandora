using Pandora.Core.Domain.Entities;
using Pandora.Core.Domain.Interfaces;

namespace Pandora.Application.Interfaces.Repositories;

public interface IRoleRepository : IAsyncRepository<Role, Guid>
{
    Task<List<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken);
}