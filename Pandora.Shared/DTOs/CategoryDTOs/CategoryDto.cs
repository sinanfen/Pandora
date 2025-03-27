using Pandora.Shared.DTOs.PasswordVaultDTOs;
using Pandora.Shared.DTOs.PersonalVaultDTOs;
using Pandora.Shared.DTOs.UserDTOs;

namespace Pandora.Shared.DTOs.CategoryDTOs;

public class CategoryDto : BaseDto<Guid>
{
    public Guid UserId { get; set; }
    public virtual UserDto UserDto { get; set; }
    public string Name { get; set; }  // Category name, e.g., Social Media, Work
    public string Description { get; set; }  // Optional category description

    // Associated vaults and Pandora's boxes
    public List<PasswordVaultDto> PasswordVaults { get; set; } = new List<PasswordVaultDto>();
    public List<PersonalVaultDto> PersonalVaultDtos { get; set; } = new List<PersonalVaultDto>();

    // Optional metadata for frontend display
    public int TotalVaults => PasswordVaults.Count;  // Count of vaults in this category
    public int TotalPersonalVaults => PersonalVaultDtos.Count;  // Count of Pandora boxes in this category
}
