namespace Pandora.Core.Domain.Entities;

// Core/Domain/Entities/PasswordVault.cs
public class PasswordVault : Entity<Guid>
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    // Name of the site, e.g., "Facebook"
    public string SecureSiteName { get; set; }

    // Encrypted username or email used for login
    public string SecureUsernameOrEmail { get; set; }  // AES-encrypted

    // Hashed password for the site (irreversible)
    public string PasswordHash { get; set; }  // SHA512 hash

    // Optional notes about this credential (encrypted for privacy)
    public string SecureNotes { get; set; }  // AES-encrypted

    // Date when the password was last changed
    public DateTime? LastPasswordChangeDate { get; set; }

    // Optional expiration date for the password
    public DateTime? PasswordExpirationDate { get; set; }

    // Optional field for linking the credential to a category
    public Guid? CategoryId { get; set; }
    public Category Category { get; set; }

    public PasswordVault() : base(Guid.NewGuid())
    {
        // Default constructor
    }
}
