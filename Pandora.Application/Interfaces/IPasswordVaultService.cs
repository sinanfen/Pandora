﻿using Microsoft.EntityFrameworkCore.Query;
using Pandora.Application.Interfaces.Results;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using Pandora.Shared.DTOs.PasswordVaultDTOs;
using System.Linq.Expressions;

namespace Pandora.Application.Interfaces;

public interface IPasswordVaultService
{
    Task<PasswordVaultDto?> GetAsync(
Expression<Func<PasswordVault, bool>> predicate,
Func<IQueryable<PasswordVault>, IIncludableQueryable<PasswordVault, object>>? include = null,
bool withDeleted = false,
bool enableTracking = true,
CancellationToken cancellationToken = default);
    Task<Paginate<PasswordVaultDto>?> GetListAsync(
      Expression<Func<PasswordVault, bool>>? predicate = null,
      Func<IQueryable<PasswordVault>, IOrderedQueryable<PasswordVault>>? orderBy = null,
      Func<IQueryable<PasswordVault>, IIncludableQueryable<PasswordVault, object>>? include = null,
      int index = 0,
      int size = 10,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default);
    Task<IDataResult<PasswordVaultDto>> AddAsync(PasswordVaultAddDto dto, CancellationToken cancellationToken);
    Task<IDataResult<PasswordVaultDto>> UpdateAsync(PasswordVaultUpdateDto dto, CancellationToken cancellationToken);
    Task<IResult> DeleteAsync(Guid passwordVaultId, CancellationToken cancellationToken);
    Task<PasswordVaultDto> GetByIdAsync(Guid passwordVaultId, CancellationToken cancellationToken);
    Task<IDataResult<PasswordVaultDto>> GetByIdAndUserAsync(Guid passwordVaultId, Guid userId, CancellationToken cancellationToken);
    Task<List<PasswordVaultDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false);
    Task<List<PasswordVaultDto>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken, bool withDeleted = false);
}