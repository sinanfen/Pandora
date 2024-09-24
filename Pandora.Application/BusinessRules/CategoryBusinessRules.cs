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

    // Kategori adı benzersiz mi kontrol et (insert sırasında)
    public async Task CategoryNameCannotBeDuplicatedWhenInserted(string categoryName)
    {
        var exists = await _categoryRepository.AnyAsync(c => c.Name.ToUpper() == categoryName.ToUpper());
        if (exists)
        {
            throw new BusinessException("Bu kategori adı ile zaten bir kayıt oluşturulmuş.");
        }
    }

    // Kategori adı benzersiz mi kontrol et (update sırasında)
    public async Task CategoryNameCannotBeDuplicatedWhenUpdated(Guid categoryId, string categoryName)
    {
        var exists = await _categoryRepository.AnyAsync(c => c.Name.ToUpper() == categoryName.ToUpper() && c.Id != categoryId);
        if (exists)
        {
            throw new BusinessException("Bu kategori adı ile başka bir kayıt var.");
        }
    }

    // Kategori güncellenebilir mi kontrol et
    public void CategoryCannotBeDeletedIfLinkedToData(Category category)
    {
        if (category.PasswordVaults.Any())
        {
            throw new BusinessException("Bu kategoriye bağlı veriler var, bu nedenle silinemez.");
        }
        if (category.PersonalVaults.Any())
        {
            throw new BusinessException("Bu kategoriye bağlı veriler var, bu nedenle silinemez.");
        }
    }
}

