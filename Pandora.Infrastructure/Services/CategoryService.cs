using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Pandora.Core.Domain.Paging;
using FluentValidation;
using Pandora.Application.BusinessRules;
using Pandora.Shared.DTOs.CategoryDTOs;
using Pandora.Application.Interfaces.Results;
using Pandora.Infrastructure.Utilities.Results.Implementations;

namespace Pandora.Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly CategoryBusinessRules _categoryBusinessRules;
    private readonly IValidator<CategoryAddDto> _categoryAddDtoValidator;
    private readonly IValidator<CategoryUpdateDto> _categoryUpdateDtoValidator;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryService> logger, CategoryBusinessRules categoryBusinessRules, IValidator<CategoryAddDto> categoryAddDtoValidator, IValidator<CategoryUpdateDto> categoryUpdateDtoValidator)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
        _categoryBusinessRules = categoryBusinessRules;
        _categoryAddDtoValidator = categoryAddDtoValidator;
        _categoryUpdateDtoValidator = categoryUpdateDtoValidator;
    }

    public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _categoryAddDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new DataResult<CategoryDto>(ResultStatus.Error, "Doğrulama hatası: " +
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), null);
            }

            await _categoryBusinessRules.CategoryNameCannotBeDuplicatedWhenInserted(dto.Name);

            Category category = _mapper.Map<Category>(dto);
            await _categoryRepository.AddAsync(category, cancellationToken);

            return new DataResult<CategoryDto>(ResultStatus.Success, "Kategori başarıyla kaydedildi.", _mapper.Map<CategoryDto>(category));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to add the category. Details: {ExceptionMessage}", nameof(AddAsync), ex.Message);
            return new DataResult<CategoryDto>(ResultStatus.Error, ex.Message, null);
        }
    }

    public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _categoryUpdateDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new DataResult<CategoryDto>(ResultStatus.Error, "Doğrulama hatası: " +
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), null);
            }

            await _categoryBusinessRules.CategoryNameCannotBeDuplicatedWhenUpdated(dto.Id, dto.Name);

            var category = await _categoryRepository.GetAsync(u => u.Id == dto.Id);
            if (category == null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Error, "Kategori bulunamadı.", null);
            }

            _mapper.Map(dto, category);
            await _categoryRepository.UpdateAsync(category);
            var updatedCategoryDto = _mapper.Map<CategoryDto>(category);

            return new DataResult<CategoryDto>(ResultStatus.Success, "Kategori başarıyla güncellendi.", updatedCategoryDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to update category. Details: {ExceptionMessage}", nameof(UpdateAsync), ex.Message);
            return new DataResult<CategoryDto>(ResultStatus.Error, "Kategori güncellenirken hata oluştu.", data: null, ex);
        }
    }

    public async Task<IResult> DeleteAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        try
        {
            Category? category = await _categoryRepository.GetAsync(x => x.Id == categoryId, cancellationToken: cancellationToken);
            if (category == null)
            {
                return new Result(ResultStatus.Warning, "Kullanıcı bulunamadı.");
            }
            await _categoryRepository.DeleteAsync(category, cancellationToken: cancellationToken);
            return new Result(ResultStatus.Success, "Kullanıcı başarıyla silindi.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get category. Details: {ExceptionMessage}", nameof(GetByIdAsync), ex.Message);
            return new Result(ResultStatus.Error, "Failed to get category.", ex);
        }
    }

    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false)
    {
        try
        {
            var pagedData = await _categoryRepository.GetListAsync(cancellationToken: cancellationToken);
            var categoryDtos = _mapper.Map<List<CategoryDto>>(pagedData.Items);
            return categoryDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get list of category. Details: {ExceptionMessage}", nameof(GetAllAsync), ex.Message);
            throw;
        }
    }

    public async Task<List<CategoryDto>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken, bool withDeleted = false)
    {
        try
        {
            var pagedData = await _categoryRepository.GetListAsync(x => x.UserId == userId, cancellationToken: cancellationToken);
            var categoryDtos = _mapper.Map<List<CategoryDto>>(pagedData.Items);
            return categoryDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get list of category. Details: {ExceptionMessage}", nameof(GetAllAsync), ex.Message);
            throw;
        }
    }

    public async Task<CategoryDto?> GetAsync(Expression<Func<Category, bool>> predicate, Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        try
        {
            Category? category = await _categoryRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
            return _mapper.Map<CategoryDto>(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get category. Details: {ExceptionMessage}", nameof(GetAsync), ex.Message);
            throw;
        }
    }

    public async Task<CategoryDto> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        try
        {
            Category? category = await _categoryRepository.GetAsync(x => x.Id == categoryId, cancellationToken: cancellationToken);
            return _mapper.Map<CategoryDto>(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get category. Details: {ExceptionMessage}", nameof(GetByIdAsync), ex.Message);
            throw;
        }
    }

    public async Task<CategoryDto> GetByIdAndUserAsync(Guid categoryId, Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            Category? category = await _categoryRepository.GetAsync(x => x.Id == categoryId && x.UserId == userId, cancellationToken: cancellationToken);
            return _mapper.Map<CategoryDto>(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get category. Details: {ExceptionMessage}", nameof(GetByIdAsync), ex.Message);
            throw;
        }
    }

    public async Task<Paginate<CategoryDto>?> GetListAsync(Expression<Func<Category, bool>>? predicate = null, Func<IQueryable<Category>, IOrderedQueryable<Category>>? orderBy = null, Func<IQueryable<Category>, IIncludableQueryable<Category, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        try
        {
            IPaginate<Category> categoryList = await _categoryRepository.GetListAsync(predicate, orderBy, include, index, size, withDeleted, enableTracking, cancellationToken);
            return _mapper.Map<Paginate<CategoryDto>>(categoryList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get paged list of category. Details: {ExceptionMessage}", nameof(GetListAsync), ex.Message);
            throw;
        }
    }
}
