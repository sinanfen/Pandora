
namespace Pandora.Application.DTOs.PasswordVaultDTOs;

public class PasswordVaultDto : BaseDto<Guid>
{
    // User details
    public Guid UserId { get; set; }

    // Vault details
    public string SecureSiteName { get; set; }  // e.g., "Facebook"
    public string SecureUsernameOrEmail { get; set; }  // AES-encrypted username/email
    public string SecureNotes { get; set; }  // AES-encrypted notes
    public DateTime? LastPasswordChangeDate { get; set; }  // Last time password was changed
    public DateTime? PasswordExpirationDate { get; set; }  // Optional expiration date for the password

    // Category details
    public Guid? CategoryId { get; set; }
}
