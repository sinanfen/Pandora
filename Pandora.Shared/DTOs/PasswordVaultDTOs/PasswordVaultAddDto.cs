
namespace Pandora.Shared.DTOs.PasswordVaultDTOs;

public class PasswordVaultAddDto : IPasswordVaultDto
{
    // User details
    public Guid UserId { get; set; }  // The user who adds the vault

    // Vault details
    public string SiteName { get; set; }  // e.g., "Facebook"
    public string UsernameOrEmail { get; set; }  // Username or email (to be AES-encrypted)
    public string Password { get; set; }  // Plain text olarak değil, AES-encrypted
    public string PasswordRepeat { get; set; }  // Password repeat
    public string Notes { get; set; }  // Optional notes (to be AES-encrypted)
    public DateTime? PasswordExpirationDate { get; set; }  // Optional expiration date for the password

    // Category
    public Guid CategoryId { get; set; }  // category assignment
}
