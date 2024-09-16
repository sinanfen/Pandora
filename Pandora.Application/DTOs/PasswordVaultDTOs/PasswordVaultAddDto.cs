
namespace Pandora.Application.DTOs.PasswordVaultDTOs;

public class PasswordVaultAddDto
{
    // User details
    public Guid UserId { get; set; }  // The user who adds the vault

    // Vault details
    public string SiteName { get; set; }  // e.g., "Facebook"
    public string UsernameOrEmail { get; set; }  // Username or email (to be AES-encrypted)
    public string Password { get; set; }  // Password (to be hashed)
    public string Notes { get; set; }  // Optional notes (to be AES-encrypted)
    public DateTime? PasswordExpirationDate { get; set; }  // Optional expiration date for the password

    // Category
    public Guid? CategoryId { get; set; }  // Optional category assignment
}
