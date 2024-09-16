using Pandora.Application.Interfaces.Repositories;
using Pandora.Core.Domain.Entities;
using Pandora.Infrastructure.Data.Contexts;
using Pandora.Infrastructure.Repositories.Generic;

namespace Pandora.Infrastructure.Repositories;

public class CategoryRepository : EfRepositoryBase<Category, Guid, PandoraDbContext>, ICategoryRepository
{
    private readonly PandoraDbContext _context;

    public CategoryRepository(PandoraDbContext context) : base(context)
    {
        _context = context;
    }
}
