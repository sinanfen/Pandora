
namespace Pandora.Application.DTOs.PandoraBoxDTOs;

public class PandoraBoxAddDto
{
    // User details
    public Guid UserId { get; set; }  // The user creating the box

    // Box content
    public string Title { get; set; }  // Box title (e.g., "Links to Watch Later")
    public string Content { get; set; }  // Unencrypted content (to be AES-encrypted)
    public string Url { get; set; }  // Unencrypted URL (to be AES-encrypted)
    public string MediaFile { get; set; }  // Unencrypted media file path (to be AES-encrypted)
    public string Summary { get; set; }  // Optional summary of the content
    public ICollection<string> Tags { get; set; }  // Tags for organization

    // Category
    public Guid? CategoryId { get; set; }  // Optional category assignment

    // Metadata
    public DateTime? ExpirationDate { get; set; }  // Optional expiration date
    public bool IsFavorite { get; set; }  // Mark as favorite
}