using Pandora.Shared.DTOs.PasswordVaultDTOs;
using Pandora.Shared.DTOs.PersonalVaultDTOs;
using Pandora.Shared.DTOs.CategoryDTOs;
using Pandora.Shared.DTOs.UserRoleDTOs;

namespace Pandora.Shared.DTOs.UserDTOs;

public class UserDto : BaseDto<Guid>
{
    // Basic identity fields
    public string Username { get; set; }
    public string NormalizedUsername { get; set; } // For case-insensitive username handling
    public string Email { get; set; }
    public string NormalizedEmail { get; set; } // For case-insensitive email handling
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public DateTime LastLoginDate { get; set; }
    public DateTime? LastPasswordChangeDate { get; set; }

    // Navigation Properties - Password Vaults and Pandora Boxes
    public List<PasswordVaultDto> PasswordVaults { get; set; } = new List<PasswordVaultDto>();
    public List<PersonalVaultDto> PersonalVaults { get; set; } = new List<PersonalVaultDto>();
    public List<CategoryDto> CategoryDtos { get; set; } = new List<CategoryDto>();
    public List<UserRoleDto> UserRoleDtos { get; set; } = new List<UserRoleDto>();

    // Additional Fields for Frontend Display
    public bool IsLockedOut => LockoutEnd.HasValue && LockoutEnd > DateTime.UtcNow;
    public int TotalVaults => PasswordVaults?.Count ?? 0; // Total number of password vaults
    public int TotalPersonalVaults => PersonalVaults?.Count ?? 0; // Total number of Pandora box entries

    // Display-friendly full name (for individual users)
    public string FullName => $"{FirstName} {LastName}".Trim();
}
