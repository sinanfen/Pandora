namespace Pandora.Application.Interfaces.Security;

public interface ITempTokenService
{
    /// <summary>
    /// Store temporary token for 2FA login flow
    /// </summary>
    Task<string> CreateTempTokenAsync(Guid userId, string ipAddress, string userAgent, TimeSpan? expiry = null);
    
    /// <summary>
    /// Validate and consume temporary token
    /// </summary>
    Task<TempTokenData?> ValidateAndConsumeTempTokenAsync(string tempToken);
    
    /// <summary>
    /// Remove expired tokens (cleanup)
    /// </summary>
    Task CleanupExpiredTokensAsync();
}

public class TempTokenData
{
    public Guid UserId { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
} 