
namespace Pandora.Shared.DTOs.PasswordVaultDTOs;

public class PasswordVaultAddDto : IPasswordVaultDto
{
    // Vault details
    public string SiteName { get; set; }  // e.g., "Facebook"
    public string UsernameOrEmail { get; set; }  // Username or email (to be AES-encrypted)
    public string Password { get; set; }  // Plain text olarak değil, AES-encrypted
    public string PasswordRepeat { get; set; }  // Password repeat
    public string Notes { get; set; }  // Optional notes (to be AES-encrypted)

    // Category
    public Guid CategoryId { get; set; }  // category assignment
}
