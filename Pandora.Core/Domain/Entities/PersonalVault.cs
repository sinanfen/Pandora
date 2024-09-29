namespace Pandora.Core.Domain.Entities;

// Core/Domain/Entities/PersonalVault.cs
public class PersonalVault : Entity<Guid>
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    // Title of the box entry, e.g., "Links to Watch Later", "Dear Diary"
    public string SecureTitle { get; set; }
    // AES-encrypted content (e.g., links, notes, documents, or media)
    public string SecureContent { get; set; }
    // Summary or preview of the content, decrypted for display (optional)
    public string SecureSummary { get; set; }
    // Optional URL field for storing related links (AES-encrypted)
    public string SecureUrl { get; set; }
    // Encrypted field to store any media file references (optional)
    public string SecureMediaFile { get; set; }  // Path to media file (AES-encrypted)
    // Optional tags for better organization and searching
    public IList<string> SecureTags { get; set; }

    // Metadata: Date of creation and last modified date
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }

    // Field to track whether this entry is marked as important or favorite
    public bool IsFavorite { get; set; }

    // Optional expiration date for entries, e.g., auto-delete after a certain period
    public DateTime? ExpirationDate { get; set; }

    // Optional category for organization
    public Guid? CategoryId { get; set; }
    public Category Category { get; set; }

    public PersonalVault() : base(Guid.NewGuid())
    {
        SecureTags = new List<string>();
        CreatedDate = DateTime.UtcNow;
    }
}
