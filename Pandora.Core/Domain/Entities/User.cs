using Pandora.Core.Domain.Constants.Enums;

namespace Pandora.Core.Domain.Entities;

// Core/Domain/Entities/User.cs
public abstract class User : Entity<Guid>
{
    // Basic identity fields
    public string Username { get; set; }
    public string NormalizedUsername { get; set; } // Used for case-insensitive username checks
    public string Email { get; set; }
    public string NormalizedEmail { get; set; } // Used for case-insensitive email checks
    public string PasswordHash { get; set; } // Hashed password for security
    public string SecurityStamp { get; set; } // Security stamp for verifying user identity changes (optional)

    // Email confirmation and lockout fields
    public bool EmailConfirmed { get; set; } // Email confirmation flag
    public bool LockoutEnabled { get; set; } // Lockout feature flag
    public DateTime? LockoutEnd { get; set; } // If locked out, when will the lockout end?

    // Two-factor authentication
    public bool TwoFactorEnabled { get; set; }

    // User type: Individual or Corporate
    public UserType UserType { get; set; } // Enum to differentiate between Individual and Corporate

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

    public User() : base(Guid.NewGuid())
    {
        PasswordVaults = new List<PasswordVault>();
        PersonalVaults = new List<PersonalVault>();
        Categories = new List<Category>();
    }
}

