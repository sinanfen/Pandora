﻿using Microsoft.EntityFrameworkCore.Query;
using Pandora.Application.Interfaces.Results;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using Pandora.Shared.DTOs.PersonalVaultDTOs;
using System.Linq.Expressions;

namespace Pandora.Application.Interfaces;

public interface IPersonalVaultService
{
    Task<PersonalVaultDto?> GetAsync(
Expression<Func<PersonalVault, bool>> predicate,
Func<IQueryable<PersonalVault>, IIncludableQueryable<PersonalVault, object>>? include = null,
bool withDeleted = false,
bool enableTracking = true,
CancellationToken cancellationToken = default);
    Task<Paginate<PersonalVaultDto>?> GetListAsync(
      Expression<Func<PersonalVault, bool>>? predicate = null,
      Func<IQueryable<PersonalVault>, IOrderedQueryable<PersonalVault>>? orderBy = null,
      Func<IQueryable<PersonalVault>, IIncludableQueryable<PersonalVault, object>>? include = null,
      int index = 0,
      int size = 10,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default);
    Task<IDataResult<PersonalVaultDto>> AddAsync(PersonalVaultAddDto dto, CancellationToken cancellationToken);
    Task<IDataResult<PersonalVaultDto>> UpdateAsync(PersonalVaultUpdateDto dto, CancellationToken cancellationToken);
    Task<IResult> DeleteAsync(Guid personalVaultId, CancellationToken cancellationToken);
    Task<PersonalVaultDto> GetByIdAsync(Guid personalVaultId, CancellationToken cancellationToken);
    Task<PersonalVaultDto> GetByIdAndUserAsync(Guid personalVaultId, Guid userId, CancellationToken cancellationToken);
    Task<List<PersonalVaultDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false);
    Task<List<PersonalVaultDto>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken, bool withDeleted = false);
}
