using Microsoft.EntityFrameworkCore;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Core.Domain.Entities;
using Pandora.Infrastructure.Data.Contexts;
using Pandora.Infrastructure.Repositories.Generic;

namespace Pandora.Infrastructure.Repositories;

public class EmailVerificationTokenRepository : EfRepositoryBase<EmailVerificationToken, Guid, PandoraDbContext>, IEmailVerificationTokenRepository
{
    public EmailVerificationTokenRepository(PandoraDbContext context) : base(context)
    {
    }

    public async Task<EmailVerificationToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await Context.Set<EmailVerificationToken>()
            .Include(evt => evt.User)
            .FirstOrDefaultAsync(evt => evt.Token == token, cancellationToken);
    }

    public async Task<List<EmailVerificationToken>> GetValidTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<EmailVerificationToken>()
            .Where(evt => evt.UserId == userId && !evt.IsUsed && evt.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(evt => evt.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<EmailVerificationToken>> GetTokensByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await Context.Set<EmailVerificationToken>()
            .Include(evt => evt.User)
            .Where(evt => evt.Email.ToLower() == email.ToLower())
            .OrderByDescending(evt => evt.CreatedDate)
            .Take(10) // Limit for security
            .ToListAsync(cancellationToken);
    }

    public async Task InvalidateAllUserTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userTokens = await Context.Set<EmailVerificationToken>()
            .Where(evt => evt.UserId == userId && !evt.IsUsed)
            .ToListAsync(cancellationToken);

        foreach (var token in userTokens)
        {
            token.IsUsed = true;
            token.UpdatedDate = DateTime.UtcNow;
        }

        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteExpiredTokensAsync(CancellationToken cancellationToken = default)
    {
        var expiredTokens = await Context.Set<EmailVerificationToken>()
            .Where(evt => evt.ExpiresAt <= DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        Context.Set<EmailVerificationToken>().RemoveRange(expiredTokens);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<EmailVerificationToken>> GetPendingTokensByIpAddressAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        return await Context.Set<EmailVerificationToken>()
            .Include(evt => evt.User)
            .Where(evt => evt.IpAddress == ipAddress && !evt.IsUsed)
            .OrderByDescending(evt => evt.CreatedDate)
            .Take(5) // Limit for performance
            .ToListAsync(cancellationToken);
    }
} 