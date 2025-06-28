using Pandora.Shared.DTOs.PasswordVaultDTOs;
using Pandora.Shared.DTOs.PersonalVaultDTOs;
using Pandora.Shared.DTOs.UserDTOs;

namespace Pandora.Shared.DTOs.CategoryDTOs;

public class CategoryDto : BaseDto<Guid>
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserFullName { get; set; } = string.Empty;  // FirstName + LastName
    public string Name { get; set; }  // Category name, e.g., Social Media, Work
    public string Description { get; set; }  // Optional category description
}
