
namespace Pandora.Application.DTOs.PasswordVaultDTOs;

public class PasswordVaultUpdateDto : BaseDto<Guid>, IPasswordVaultDto
{
    public Guid UserId { get; set; }
    // Vault details
    public string SiteName { get; set; }  // e.g., "Facebook"
    public string UsernameOrEmail { get; set; }  // Username or email (to be AES-encrypted)
    public string Notes { get; set; }  // Optional notes (to be AES-encrypted)
    public DateTime? PasswordExpirationDate { get; set; }  // Optional expiration date for the password
    public DateTime? LastPasswordChangeDate { get; set; }  // Last time password was changed

    // Password fields
    public string CurrentPassword { get; set; }  // Mevcut parolanız (doğrulamak için)
    public string NewPassword { get; set; }  // Güncellenmek istenen yeni parola
    public string NewPasswordRepeat { get; set; }  // Yeni parolanın tekrarı

    // Category
    public Guid? CategoryId { get; set; }  // Update category if needed
}
