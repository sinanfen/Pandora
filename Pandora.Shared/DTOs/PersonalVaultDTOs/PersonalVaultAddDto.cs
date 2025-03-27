
using Pandora.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Pandora.Shared.DTOs.PersonalVaultDTOs;

public class PersonalVaultAddDto : IPersonalVaultDto
{
    // User details
    public Guid UserId { get; set; }  // The user creating the box
    // Box content
    public string Title { get; set; }  // Box title (e.g., "Links to Watch Later")
    public string Content { get; set; }  // Unencrypted content (to be AES-encrypted)
    public string Url { get; set; }  // Unencrypted URL (to be AES-encrypted)
    public string MediaFile { get; set; }  // Unencrypted media file path (to be AES-encrypted)
    public string Summary { get; set; }  // Optional summary of the content
    public IList<string> Tags { get; set; }  // Tags for organization
    [Required]
    public bool IsLocked { get; set; } // Kullanıcı "kilitle" seçeneğini işaretler
    [RequiredIfTrue(nameof(IsLocked))] // Özel bir validation attribute 
    [MinimumDateOffset(1, TimeUnit.Month)] // Özel validation (en az 1 ay sonrası)
    public DateTime? UnlockDate { get; set; } // Kilit açılma tarihi
    // Category
    public Guid? CategoryId { get; set; }  // Optional category assignment
    // Metadata
    public DateTime? ExpirationDate { get; set; }  // Optional expiration date
    public bool IsFavorite { get; set; }  // Mark as favorite
}