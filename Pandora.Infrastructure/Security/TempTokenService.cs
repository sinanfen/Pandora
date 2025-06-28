using System.Collections.Concurrent;
using System.Security.Cryptography;
using Pandora.Application.Interfaces.Security;

namespace Pandora.Infrastructure.Security;

public class TempTokenService : ITempTokenService
{
    private readonly ConcurrentDictionary<string, TempTokenData> _tokens = new();
    private readonly Timer _cleanupTimer;
    private readonly TimeSpan _defaultExpiry = TimeSpan.FromMinutes(5); // 5 minutes default

    public TempTokenService()
    {
        // Cleanup expired tokens every minute
        _cleanupTimer = new Timer(async _ => await CleanupExpiredTokensAsync(), null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
    }

    public Task<string> CreateTempTokenAsync(Guid userId, string ipAddress, string userAgent, TimeSpan? expiry = null)
    {
        var token = GenerateSecureToken();
        var expiryTime = expiry ?? _defaultExpiry;
        
        var tokenData = new TempTokenData
        {
            UserId = userId,
            IpAddress = ipAddress,
            UserAgent = userAgent,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.Add(expiryTime)
        };
        
        _tokens[token] = tokenData;
        
        return Task.FromResult(token);
    }

    public Task<TempTokenData?> ValidateAndConsumeTempTokenAsync(string tempToken)
    {
        if (string.IsNullOrEmpty(tempToken))
            return Task.FromResult<TempTokenData?>(null);

        if (!_tokens.TryRemove(tempToken, out var tokenData))
            return Task.FromResult<TempTokenData?>(null);

        // Check if token has expired
        if (DateTime.UtcNow > tokenData.ExpiresAt)
            return Task.FromResult<TempTokenData?>(null);

        return Task.FromResult<TempTokenData?>(tokenData);
    }

    public Task CleanupExpiredTokensAsync()
    {
        var now = DateTime.UtcNow;
        var expiredTokens = _tokens.Where(kvp => now > kvp.Value.ExpiresAt).Select(kvp => kvp.Key).ToList();
        
        foreach (var expiredToken in expiredTokens)
        {
            _tokens.TryRemove(expiredToken, out _);
        }
        
        return Task.CompletedTask;
    }

    private static string GenerateSecureToken()
    {
        var tokenBytes = new byte[32]; // 256-bit token
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(tokenBytes);
        return Convert.ToBase64String(tokenBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }

    public void Dispose()
    {
        _cleanupTimer?.Dispose();
    }
} 