using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Pandora.Application.DTOs.PasswordVaultDTOs;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Security.Interfaces;
using Pandora.Application.Utilities.Results.Interfaces;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Pandora.Application.Services;

public class PasswordVaultService : IPasswordVaultService
{
    private readonly IPasswordVaultRepository _passwordVaultRepository;
    private readonly IMapper _mapper;
    private readonly IHasher _hasher;
    private readonly IEncryption _encryption;

    public PasswordVaultService(IPasswordVaultRepository passwordVaultRepository, IMapper mapper, IHasher hasher, IEncryption encryption)
    {
        _passwordVaultRepository = passwordVaultRepository;
        _mapper = mapper;
        _hasher = hasher;
        _encryption = encryption;
    }

    public Task<IDataResult<PasswordVaultDto>> AddAsync(PasswordVaultUpdateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IResult> DeleteAsync(Guid passwordVaultId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<PasswordVaultDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false)
    {
        throw new NotImplementedException();
    }

    public Task<PasswordVaultDto?> GetAsync(Expression<Func<PasswordVault, bool>> predicate, Func<IQueryable<PasswordVault>, IIncludableQueryable<PasswordVault, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<PasswordVaultDto> GetByIdAsync(Guid passwordVaultId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Paginate<PasswordVaultDto>?> GetListAsync(Expression<Func<PasswordVault, bool>>? predicate = null, Func<IQueryable<PasswordVault>, IOrderedQueryable<PasswordVault>>? orderBy = null, Func<IQueryable<PasswordVault>, IIncludableQueryable<PasswordVault, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IDataResult<PasswordVaultDto>> UpdateAsync(PasswordVaultUpdateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}