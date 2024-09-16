using Pandora.Application.Interfaces.Repositories;
using Pandora.Core.Domain.Entities;
using Pandora.Infrastructure.Data.Contexts;
using Pandora.Infrastructure.Repositories.Generic;

namespace Pandora.Infrastructure.Repositories;

public class PandoraBoxRepository : EfRepositoryBase<PandoraBox, Guid, PandoraDbContext>, IPandoraBoxRepository
{
    private readonly PandoraDbContext _context;

    public PandoraBoxRepository(PandoraDbContext context) : base(context)
    {
        _context = context;
    }
}
