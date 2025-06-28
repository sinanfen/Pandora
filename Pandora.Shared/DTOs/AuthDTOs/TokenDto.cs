namespace Pandora.Shared.DTOs.AuthDTOs;

/// <summary>
/// Response model containing both access and refresh tokens
/// </summary>
public class TokenDto
{
    /// <summary>
    /// JWT Access token (short-lived)
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;
    
    /// <summary>
    /// Refresh token (long-lived)
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
    
    /// <summary>
    /// When the access token expires (Unix timestamp)
    /// </summary>
    public long AccessTokenExpiresAt { get; set; }
    
    /// <summary>
    /// When the refresh token expires (Unix timestamp)
    /// </summary>
    public long RefreshTokenExpiresAt { get; set; }
    
    /// <summary>
    /// Token type (always "Bearer")
    /// </summary>
    public string TokenType { get; set; } = "Bearer";
    
    // 2FA Support
    public bool RequiresTwoFactor { get; set; }
    public string? TempToken { get; set; } // Used for 2FA flow
}

/// <summary>
/// Request model for refreshing access token
/// </summary>
public class RefreshTokenDto
{
    /// <summary>
    /// The refresh token to use for getting new access token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// Request model for revoking refresh token
/// </summary>
public class RevokeTokenDto
{
    /// <summary>
    /// The refresh token to revoke
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
    
    /// <summary>
    /// Reason for revocation (optional)
    /// </summary>
    public string? Reason { get; set; }
}

/// <summary>
/// Response model for active sessions
/// </summary>
public class SessionDto
{
    public Guid Id { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsCurrentSession { get; set; }
} 