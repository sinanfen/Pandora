
namespace Pandora.Shared.DTOs.PasswordVaultDTOs;

public class PasswordVaultDto : BaseDto<Guid>
{
    // User details
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;  // Username from navigation property

    // Vault details
    public string SecureSiteName { get; set; }  // e.g., "Facebook"
    public string SecureUsernameOrEmail { get; set; }  // AES-encrypted username/email
    public string SecureNotes { get; set; }  // AES-encrypted notes
    public string Password { get; set; }  // Plain text olarak değil, AES-encrypted
    public DateTime? LastPasswordChangeDate { get; set; }  // Last time password was changed

    // Category details
    public Guid? CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}
