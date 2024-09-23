using Microsoft.EntityFrameworkCore.Query;
using Pandora.Application.DTOs.PandoraBoxDTOs;
using Pandora.Application.Utilities.Results.Interfaces;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Pandora.Application.Interfaces;

public interface IPandoraBoxService
{
    Task<PandoraBoxDto?> GetAsync(
Expression<Func<PandoraBox, bool>> predicate,
Func<IQueryable<PandoraBox>, IIncludableQueryable<PandoraBox, object>>? include = null,
bool withDeleted = false,
bool enableTracking = true,
CancellationToken cancellationToken = default);
    Task<Paginate<PandoraBoxDto>?> GetListAsync(
      Expression<Func<PandoraBox, bool>>? predicate = null,
      Func<IQueryable<PandoraBox>, IOrderedQueryable<PandoraBox>>? orderBy = null,
      Func<IQueryable<PandoraBox>, IIncludableQueryable<PandoraBox, object>>? include = null,
      int index = 0,
      int size = 10,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default);
    Task<IDataResult<PandoraBoxDto>> AddAsync(PandoraBoxUpdateDto dto, CancellationToken cancellationToken);
    Task<IDataResult<PandoraBoxDto>> UpdateAsync(PandoraBoxUpdateDto dto, CancellationToken cancellationToken);
    Task<IResult> DeleteAsync(Guid pandoraBoxId, CancellationToken cancellationToken);
    Task<PandoraBoxDto> GetByIdAsync(Guid pandoraBoxId, CancellationToken cancellationToken);
    Task<List<PandoraBoxDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false);
}
