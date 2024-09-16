
namespace Pandora.Application.DTOs.PandoraBoxDTOs;

public class PandoraBoxDto : BaseDto<Guid>
{
    // User details
    public Guid UserId { get; set; }
    public string Username { get; set; }  // Optionally display the username

    // Box content
    public string Title { get; set; }  // Box title (e.g., "Links to Watch Later")
    public string EncryptedContent { get; set; }  // AES-encrypted content (e.g., notes, links)
    public string Summary { get; set; }  // Optional preview of the content
    public string EncryptedUrl { get; set; }  // AES-encrypted URL, if available
    public string EncryptedMediaFile { get; set; }  // AES-encrypted media file path, if any

    // Metadata
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public DateTime? ExpirationDate { get; set; }

    // Tagging and favorite feature
    public ICollection<string> Tags { get; set; }  // Tags for organization
    public bool IsFavorite { get; set; }  // Whether the entry is marked as favorite

    // Category details
    public Guid? CategoryId { get; set; }
    public string CategoryName { get; set; }  // Name of the associated category, if any
}
