
using Pandora.Shared.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Pandora.Shared.DTOs.PersonalVaultDTOs;

public class PersonalVaultAddDto : IPersonalVaultDto
{
    // Box content
    public string Title { get; set; }  // Box title (e.g., "Links to Watch Later")
    public string Content { get; set; }  // Unencrypted content (to be AES-encrypted)
    public string Url { get; set; }  // Unencrypted URL (to be AES-encrypted)
    public string? MediaFile { get; set; }  // Unencrypted media file path (to be AES-encrypted)
    public string Summary { get; set; }  // Optional summary of the content
    public IList<string> Tags { get; set; }  // Tags for organization
    [Required]
    public bool IsLocked { get; set; } // Kullanıcı "kilitle" seçeneğini işaretler
    [RequiredIfTrue(nameof(IsLocked))] // Özel bir validation attribute 
    [DateRangeOffset(7, TimeUnit.Day, 1, TimeUnit.Year)] // 7 gün - 1 yıl arası
    public DateTime? UnlockDate { get; set; } // Kilit açılma tarihi
    public bool IsShareable { get; set; } = false; // Zaman kapsülü paylaşılabilir mi?
    // Category
    public Guid? CategoryId { get; set; }  // Optional category assignment
    // Metadata
    public DateTime? ExpirationDate { get; set; }  // Optional expiration date
    public bool IsFavorite { get; set; }  // Mark as favorite
}