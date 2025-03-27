
namespace Pandora.Shared.DTOs.PersonalVaultDTOs;

public class PersonalVaultDto : BaseDto<Guid>
{
    // User details
    public Guid UserId { get; set; }
    public string Username { get; set; }  // Optionally display the username
    // Box content
    public string SecureTitle { get; set; }  // Box title (e.g., "Links to Watch Later")
    public string SecureContent { get; set; }  // AES-encrypted content (e.g., notes, links)
    public string SecureSummary { get; set; }  // Optional preview of the content
    public string SecureUrl { get; set; }  // AES-encrypted URL, if available
    public string SecureMediaFile { get; set; }  // AES-encrypted media file path, if any
    public bool IsLocked { get; set; } // Varsayılan değer false
    public DateTime? UnlockDate { get; set; } // Kilit açılma tarihi (en az 1 ay sonrası)
    // Metadata
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
    // Tagging and favorite feature
    public IList<string> SecureTags { get; set; }  // Tags for organization
    public bool IsFavorite { get; set; }  // Whether the entry is marked as favorite
    // Category details
    public Guid? CategoryId { get; set; }
    public string CategoryName { get; set; }  // Name of the associated category, if any
}
