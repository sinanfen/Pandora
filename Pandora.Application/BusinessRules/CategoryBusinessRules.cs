using Pandora.Application.Interfaces.Repositories;
using Pandora.Core.Domain.Entities;
using Pandora.CrossCuttingConcerns.ExceptionHandling;

namespace Pandora.Application.BusinessRules;

public class CategoryBusinessRules
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryBusinessRules(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    // Check if category name is unique during insert
    public async Task CategoryNameCannotBeDuplicatedWhenInserted(string categoryName)
    {
        var exists = await _categoryRepository.AnyAsync(c => c.Name.ToUpper() == categoryName.ToUpper());
        if (exists)
        {
            throw new BusinessException("A category with this name already exists.");
        }
    }

    // Check if category name is unique during update
    public async Task CategoryNameCannotBeDuplicatedWhenUpdated(Guid categoryId, string categoryName)
    {
        var exists = await _categoryRepository.AnyAsync(c => c.Name.ToUpper() == categoryName.ToUpper() && c.Id != categoryId);
        if (exists)
        {
            throw new BusinessException("Another category with this name already exists.");
        }
    }

    // Check if category can be deleted
    public void CategoryCannotBeDeletedIfLinkedToData(Category category)
    {
        if (category.PasswordVaults.Any())
        {
            throw new BusinessException("This category has linked data and cannot be deleted.");
        }
        if (category.PersonalVaults.Any())
        {
            throw new BusinessException("This category has linked data and cannot be deleted.");
        }
    }
}

