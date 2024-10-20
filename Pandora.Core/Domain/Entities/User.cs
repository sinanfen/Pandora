using Pandora.Core.Domain.Constants.Enums;

namespace Pandora.Core.Domain.Entities;

// Core/Domain/Entities/User.cs
public class User : Entity<Guid>
{
    // Basic identity fields
    public string Username { get; set; }
    public string NormalizedUsername { get; set; } // Used for case-insensitive username checks
    public string Email { get; set; }
    public string NormalizedEmail { get; set; } // Used for case-insensitive email checks
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; } // Hashed password for security
    public string SecurityStamp { get; set; } // Security stamp for verifying user identity changes (optional)

    // Email confirmation and lockout fields
    public bool EmailConfirmed { get; set; } // Email confirmation flag
    public bool LockoutEnabled { get; set; } // Lockout feature flag
    public DateTime? LockoutEnd { get; set; } // If locked out, when will the lockout end?

    // Two-factor authentication
    public bool TwoFactorEnabled { get; set; }

    // Optional user information
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }

    // Timestamps for auditing
    public DateTime LastLoginDate { get; set; } // Last login timestamp
    public DateTime? LastPasswordChangeDate { get; set; } // Last password change timestamp (optional)

    // Navigation properties for vaults and Pandora's boxes
    public ICollection<PasswordVault> PasswordVaults { get; set; }
    public ICollection<PersonalVault> PersonalVaults { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<UserRole> UserRoles { get; set; } // A user can have many roles

    public User() : base(Guid.NewGuid())
    {
        PasswordVaults = new List<PasswordVault>();
        PersonalVaults = new List<PersonalVault>();
        Categories = new List<Category>();
        UserRoles = new List<UserRole>();
    }
}
