using Pandora.Core.Domain.Entities;
using Pandora.Core.Domain.Interfaces;

namespace Pandora.Application.Interfaces.Repositories;

public interface IRefreshTokenRepository : IAsyncRepository<RefreshToken, Guid>, IRepository<RefreshToken, Guid>
{
    /// <summary>
    /// Get refresh token by token string
    /// </summary>
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get all valid refresh tokens for a user
    /// </summary>
    Task<List<RefreshToken>> GetValidTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Revoke all refresh tokens for a user (for logout all devices)
    /// </summary>
    Task RevokeAllUserTokensAsync(Guid userId, string reason, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Clean up expired tokens (for background job)
    /// </summary>
    Task DeleteExpiredTokensAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get tokens by IP address (for security monitoring)
    /// </summary>
    Task<List<RefreshToken>> GetTokensByIpAddressAsync(string ipAddress, CancellationToken cancellationToken = default);
} 