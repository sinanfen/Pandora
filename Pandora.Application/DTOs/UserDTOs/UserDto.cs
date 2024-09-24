using Pandora.Core.Domain.Constants.Enums;
using Pandora.Application.DTOs.PasswordVaultDTOs;
using Pandora.Application.DTOs.PersonalVaultDTOs;

namespace Pandora.Application.DTOs.UserDTOs;

public class UserDto : BaseDto<Guid>
{
    // Basic identity fields
    public string Username { get; set; }
    public string NormalizedUsername { get; set; } // For case-insensitive username handling
    public string Email { get; set; }
    public string NormalizedEmail { get; set; } // For case-insensitive email handling
    public bool EmailConfirmed { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTime? LockoutEnd { get; set; }
    public bool TwoFactorEnabled { get; set; }
    public UserType UserType { get; set; } // Individual or Corporate
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public DateTime LastLoginDate { get; set; }
    public DateTime? LastPasswordChangeDate { get; set; }

    // Corporate-specific fields
    public string CompanyName { get; set; }
    public string TaxNumber { get; set; }

    // Individual-specific fields
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Navigation Properties - Password Vaults and Pandora Boxes
    public List<PasswordVaultDto> PasswordVaults { get; set; } = new List<PasswordVaultDto>();
    public List<PersonalVaultDto> PersonalVaults { get; set; } = new List<PersonalVaultDto>();

    // Additional Fields for Frontend Display
    public bool IsLockedOut => LockoutEnd.HasValue && LockoutEnd > DateTime.UtcNow;
    public int TotalVaults => PasswordVaults?.Count ?? 0; // Total number of password vaults
    public int TotalPersonalVaults => PersonalVaults?.Count ?? 0; // Total number of Pandora box entries

    // Display-friendly full name (for individual users)
    public string FullName => $"{FirstName} {LastName}".Trim();

    // Display-friendly company details (for corporate users)
    public string CompanyDetails => UserType == UserType.Corporate
        ? $"{CompanyName} (Tax Number: {TaxNumber})"
        : null;
}
