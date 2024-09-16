using Pandora.Application.Interfaces.Repositories;
using Pandora.Core.Domain.Entities;
using Pandora.Infrastructure.Data.Contexts;
using Pandora.Infrastructure.Repositories.Generic;

namespace Pandora.Infrastructure.Repositories;

public class PasswordVaultRepository : EfRepositoryBase<PasswordVault, Guid, PandoraDbContext>, IPasswordVaultRepository
{
    private readonly PandoraDbContext _context;

    public PasswordVaultRepository(PandoraDbContext context) : base(context)
    {
        _context = context;
    }
}