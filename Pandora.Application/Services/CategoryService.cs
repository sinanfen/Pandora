using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Pandora.Application.DTOs.CategoryDTOs;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Utilities.Results.Interfaces;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using System.Linq.Expressions;

namespace Pandora.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public Task<IDataResult<CategoryDto>> AddAsync(CategoryUpdateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IResult> DeleteAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false)
    {
        throw new NotImplementedException();
    }

    public Task<CategoryDto?> GetAsync(Expression<Func<Category, bool>> predicate, Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Paginate<CategoryDto>?> GetListAsync(Expression<Func<Category, bool>>? predicate = null, Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null, Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto dto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
