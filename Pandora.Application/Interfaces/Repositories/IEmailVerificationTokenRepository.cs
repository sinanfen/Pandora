using Pandora.Core.Domain.Entities;
using Pandora.Core.Domain.Interfaces;

namespace Pandora.Application.Interfaces.Repositories;

public interface IEmailVerificationTokenRepository : IAsyncRepository<EmailVerificationToken, Guid>, IRepository<EmailVerificationToken, Guid>
{
    /// <summary>
    /// Get verification token by token string
    /// </summary>
    Task<EmailVerificationToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get all valid tokens for a user
    /// </summary>
    Task<List<EmailVerificationToken>> GetValidTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get tokens by email address
    /// </summary>
    Task<List<EmailVerificationToken>> GetTokensByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Invalidate all tokens for a user
    /// </summary>
    Task InvalidateAllUserTokensAsync(Guid userId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Clean up expired tokens
    /// </summary>
    Task DeleteExpiredTokensAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Get pending verification tokens for security monitoring
    /// </summary>
    Task<List<EmailVerificationToken>> GetPendingTokensByIpAddressAsync(string ipAddress, CancellationToken cancellationToken = default);
} 