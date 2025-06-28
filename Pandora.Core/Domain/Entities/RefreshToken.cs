namespace Pandora.Core.Domain.Entities;

public class RefreshToken : Entity<Guid>
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    /// <summary>
    /// The actual refresh token string (should be cryptographically secure)
    /// </summary>
    public string Token { get; set; }
    
    /// <summary>
    /// When this refresh token expires
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    
    /// <summary>
    /// Whether this token has been used (for token rotation)
    /// </summary>
    public bool IsUsed { get; set; }
    
    /// <summary>
    /// Whether this token has been revoked/invalidated
    /// </summary>
    public bool IsRevoked { get; set; }
    
    /// <summary>
    /// IP address when token was created (for security tracking)
    /// </summary>
    public string? IpAddress { get; set; }
    
    /// <summary>
    /// User agent when token was created (for device tracking)
    /// </summary>
    public string? UserAgent { get; set; }
    
    /// <summary>
    /// If this token was created by using another refresh token, this points to that token
    /// </summary>
    public Guid? ReplacedByTokenId { get; set; }
    
    /// <summary>
    /// Reason for revocation (logout, security, etc.)
    /// </summary>
    public string? RevocationReason { get; set; }

    public RefreshToken() : base(Guid.NewGuid())
    {
        Token = string.Empty;
        IsUsed = false;
        IsRevoked = false;
    }

    /// <summary>
    /// Check if this refresh token is currently valid
    /// </summary>
    public bool IsValid => !IsUsed && !IsRevoked && ExpiresAt > DateTime.UtcNow;
    
    /// <summary>
    /// Check if this token is expired
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
} 