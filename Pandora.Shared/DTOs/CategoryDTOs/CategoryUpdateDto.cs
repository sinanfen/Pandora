namespace Pandora.Shared.DTOs.CategoryDTOs;

public class CategoryUpdateDto : BaseDto<Guid>
{
    public string Name { get; set; }  // Category name, e.g., Social Media, Work
    public string Description { get; set; }  // Optional category description
}