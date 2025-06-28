using Microsoft.EntityFrameworkCore;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Core.Domain.Entities;
using Pandora.Infrastructure.Data.Contexts;
using Pandora.Infrastructure.Repositories.Generic;

namespace Pandora.Infrastructure.Repositories;

public class RefreshTokenRepository : EfRepositoryBase<RefreshToken, Guid, PandoraDbContext>, IRefreshTokenRepository
{
    public RefreshTokenRepository(PandoraDbContext context) : base(context)
    {
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await Context.Set<RefreshToken>()
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    public async Task<List<RefreshToken>> GetValidTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<RefreshToken>()
            .Where(rt => rt.UserId == userId && !rt.IsUsed && !rt.IsRevoked && rt.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(rt => rt.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task RevokeAllUserTokensAsync(Guid userId, string reason, CancellationToken cancellationToken = default)
    {
        var userTokens = await Context.Set<RefreshToken>()
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync(cancellationToken);

        foreach (var token in userTokens)
        {
            token.IsRevoked = true;
            token.RevocationReason = reason;
            token.UpdatedDate = DateTime.UtcNow;
        }

        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteExpiredTokensAsync(CancellationToken cancellationToken = default)
    {
        var expiredTokens = await Context.Set<RefreshToken>()
            .Where(rt => rt.ExpiresAt <= DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        Context.Set<RefreshToken>().RemoveRange(expiredTokens);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<RefreshToken>> GetTokensByIpAddressAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        return await Context.Set<RefreshToken>()
            .Include(rt => rt.User)
            .Where(rt => rt.IpAddress == ipAddress)
            .OrderByDescending(rt => rt.CreatedDate)
            .Take(20) // Limit for performance
            .ToListAsync(cancellationToken);
    }
} 