using Pandora.Application.Interfaces.Repositories;
using Pandora.Core.Domain.Entities;
using Pandora.Infrastructure.Data.Contexts;
using Pandora.Infrastructure.Repositories.Generic;

namespace Pandora.Infrastructure.Repositories;

public class PersonalVaultRepository : EfRepositoryBase<PersonalVault, Guid, PandoraDbContext>, IPersonalVaultRepository
{
    private readonly PandoraDbContext _context;

    public PersonalVaultRepository(PandoraDbContext context) : base(context)
    {
        _context = context;
    }
}
