using Microsoft.EntityFrameworkCore;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Core.Domain.Entities;
using Pandora.Infrastructure.Data.Contexts;
using Pandora.Infrastructure.Repositories.Generic;

namespace Pandora.Infrastructure.Repositories;

public class RoleRepository : EfRepositoryBase<Role, Guid, PandoraDbContext>, IRoleRepository
{
    private readonly PandoraDbContext _context;

    public RoleRepository(PandoraDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<List<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.UserRoles
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.Role.Name)
            .ToListAsync(cancellationToken);
    }
}