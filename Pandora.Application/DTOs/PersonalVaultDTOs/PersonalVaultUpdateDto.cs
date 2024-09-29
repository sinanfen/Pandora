
namespace Pandora.Application.DTOs.PersonalVaultDTOs;

public class PersonalVaultUpdateDto : BaseDto<Guid>, IPersonalVaultDto
{
    // User details
    public Guid UserId { get; set; }

    // Box content
    public string Title { get; set; }  // Box title (e.g., "Links to Watch Later")
    public string Content { get; set; }  // Unencrypted content (to be AES-encrypted)
    public string Url { get; set; }  // Unencrypted URL (to be AES-encrypted)
    public string MediaFile { get; set; }  // Unencrypted media file path (to be AES-encrypted)
    public string Summary { get; set; }  // Optional summary of the content
    public IList<string> Tags { get; set; }  // Tags for organization

    // Category
    public Guid? CategoryId { get; set; }  // Update category if needed

    // Metadata
    public DateTime? ExpirationDate { get; set; }  // Optional expiration date
    public bool IsFavorite { get; set; }  // Mark as favorite or not
    public DateTime? LastModifiedDate { get; set; }  // Update last modification timestamp
}
