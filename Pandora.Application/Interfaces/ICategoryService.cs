using Microsoft.EntityFrameworkCore.Query;
using Pandora.Application.Interfaces.Results;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using Pandora.Shared.DTOs.CategoryDTOs;
using System.Linq.Expressions;

namespace Pandora.Application.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto?> GetAsync(
    Expression<Func<Category, bool>> predicate,
    Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null,
    bool withDeleted = false,
    bool enableTracking = true,
    CancellationToken cancellationToken = default);
    Task<Paginate<CategoryDto>?> GetListAsync(
      Expression<Func<Category, bool>>? predicate = null,
      Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null,
      Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null,
      int index = 0,
      int size = 10,
      bool withDeleted = false,
      bool enableTracking = true,
      CancellationToken cancellationToken = default);
    Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto dto, Guid userId, CancellationToken cancellationToken);
    Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto dto, Guid userId, CancellationToken cancellationToken);
    Task<IResult> DeleteAsync(Guid categoryId, CancellationToken cancellationToken);
    Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken);
    Task<CategoryDto> GetByIdAndUserAsync(Guid categoryId, Guid userId, CancellationToken cancellationToken);
    Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false);
    Task<List<CategoryDto>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken, bool withDeleted = false);
}
