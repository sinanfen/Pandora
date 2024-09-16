
namespace Pandora.Application.DTOs.PasswordVaultDTOs;

public class PasswordVaultUpdateDto : BaseDto<Guid>
{
    // Vault details
    public string SiteName { get; set; }  // e.g., "Facebook"
    public string UsernameOrEmail { get; set; }  // Username or email (to be AES-encrypted)
    public string Notes { get; set; }  // Optional notes (to be AES-encrypted)
    public DateTime? PasswordExpirationDate { get; set; }  // Optional expiration date for the password
    public DateTime? LastPasswordChangeDate { get; set; }  // Last time password was changed

    // Category
    public Guid? CategoryId { get; set; }  // Update category if needed
}