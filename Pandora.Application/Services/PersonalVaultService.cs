using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Pandora.Application.DTOs.PersonalVaultDTOs;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Security.Interfaces;
using Pandora.Application.Utilities.Results.Interfaces;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Pandora.Application.Services;

public class PersonalVaultService : IPersonalVaultService
{
    private readonly IPersonalVaultRepository _personalVaultRepository;
    private readonly IMapper _mapper;
    private readonly IHasher _hasher;
    private readonly IEncryption _encryption;

    public PersonalVaultService(IPersonalVaultRepository personalVaultRepository, IMapper mapper, IHasher hasher, IEncryption encryption)
    {
        _personalVaultRepository = personalVaultRepository;
        _mapper = mapper;
        _hasher = hasher;
        _encryption = encryption;
    }

    public Task<IDataResult<PersonalVaultDto>> AddAsync(PersonalVaultUpdateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IResult> DeleteAsync(Guid personalVaultId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<PersonalVaultDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false)
    {
        throw new NotImplementedException();
    }

    public Task<PersonalVaultDto?> GetAsync(Expression<Func<PersonalVault, bool>> predicate, Func<IQueryable<PersonalVault>, IIncludableQueryable<PersonalVault, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PersonalVaultDto> GetByIdAsync(Guid personalVaultId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Paginate<PersonalVaultDto>?> GetListAsync(Expression<Func<PersonalVault, bool>>? predicate = null, Func<IQueryable<PersonalVault>, IOrderedQueryable<PersonalVault>>? orderBy = null, Func<IQueryable<PersonalVault>, IIncludableQueryable<PersonalVault, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IDataResult<PersonalVaultDto>> UpdateAsync(PersonalVaultUpdateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
