
namespace Pandora.Shared.DTOs.PersonalVaultDTOs;

public interface IPersonalVaultDto
{
    // Box content
    public string Title { get; set; }  // Box title (e.g., "Links to Watch Later")
    public string Content { get; set; }  // Unencrypted content (to be AES-encrypted)
    public string Url { get; set; }  // Unencrypted URL (to be AES-encrypted)
    public string MediaFile { get; set; }  // Unencrypted media file path (to be AES-encrypted)
    public string Summary { get; set; }  // Optional summary of the content
    public IList<string> Tags { get; set; }  // Tags for organization
}
