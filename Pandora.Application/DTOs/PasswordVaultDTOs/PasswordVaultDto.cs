
namespace Pandora.Application.DTOs.PasswordVaultDTOs;

public class PasswordVaultDto : BaseDto<Guid>
{
    // User details
    public Guid UserId { get; set; }
    public string Username { get; set; }  // Optionally display the username

    // Vault details
    public string SiteName { get; set; }  // e.g., "Facebook"
    public string EncryptedUsernameOrEmail { get; set; }  // AES-encrypted username/email
    public string EncryptedNotes { get; set; }  // AES-encrypted notes
    public DateTime? LastPasswordChangeDate { get; set; }  // Last time password was changed
    public DateTime? PasswordExpirationDate { get; set; }  // Optional expiration date for the password

    // Category details
    public Guid? CategoryId { get; set; }
    public string CategoryName { get; set; }  // Category name, if exists
}
