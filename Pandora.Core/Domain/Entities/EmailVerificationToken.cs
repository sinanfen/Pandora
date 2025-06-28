namespace Pandora.Core.Domain.Entities;

public class EmailVerificationToken : Entity<Guid>
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    /// <summary>
    /// Cryptographically secure verification token
    /// </summary>
    public string Token { get; set; }
    
    /// <summary>
    /// Email address to be verified
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// When this token expires (24 hours)
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    
    /// <summary>
    /// Whether this token has been used
    /// </summary>
    public bool IsUsed { get; set; }
    
    /// <summary>
    /// IP address when token was created
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// User agent when token was requested
    /// </summary>
    public string? UserAgent { get; set; }

    public EmailVerificationToken() : base(Guid.NewGuid())
    {
        Token = string.Empty;
        Email = string.Empty;
        IsUsed = false;
    }

    /// <summary>
    /// Check if this verification token is valid
    /// </summary>
    public bool IsValid => !IsUsed && ExpiresAt > DateTime.UtcNow;
    
    /// <summary>
    /// Check if this token is expired
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
} 