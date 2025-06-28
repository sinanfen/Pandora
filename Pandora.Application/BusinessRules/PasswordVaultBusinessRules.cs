using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Interfaces.Security;
using Pandora.CrossCuttingConcerns.ExceptionHandling;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using Pandora.Core.Domain.Entities;

namespace Pandora.Application.BusinessRules;

public class PasswordVaultBusinessRules
{
    private readonly IPasswordVaultRepository _passwordVaultRepository;
    private readonly IHasher _hasher;
    private readonly IEncryption _encryption;

    public PasswordVaultBusinessRules(IPasswordVaultRepository passwordVaultRepository, IHasher hasher, IEncryption encryption)
    {
        _passwordVaultRepository = passwordVaultRepository;
        _hasher = hasher;
        _encryption = encryption;
    }

    // Password strength validation
    public void PasswordMustMeetComplexityRequirements(string password)
    {
        const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$";
        if (!Regex.IsMatch(password, pattern))
        {
            throw new BusinessException("Password must be at least 8 characters long and contain an uppercase letter and a digit.");
        }
    }

    // Check if password is expired
    public void CheckPasswordExpiration(PasswordVault vault)
    {
        if (vault.LastPasswordChangeDate.HasValue && vault.LastPasswordChangeDate.Value.AddDays(90) < DateTime.UtcNow)
        {
            throw new BusinessException("This password has not been changed for more than 90 days.");
        }
    }

    // Check password expiration date
    public void CheckPasswordExpirationDate(PasswordVault vault)
    {
        if (vault.PasswordExpirationDate.HasValue && vault.PasswordExpirationDate.Value < DateTime.UtcNow)
        {
            throw new BusinessException("This password has expired.");
        }
    }

    // Check for password reuse
    public async Task PasswordCannotBeReused(Guid userId, string siteName, string newPasswordHash)
    {
        var exists = await _passwordVaultRepository.AnyAsync(pv => pv.UserId == userId && pv.SecureSiteName == siteName && pv.PasswordHash == newPasswordHash);
        if (exists)
        {
            throw new BusinessException("This password has been used before.");
        }
    }

    // Validate password vault exists and user access
    public async Task ValidatePasswordVaultAccess(Guid vaultId, Guid userId)
    {
        var vault = await _passwordVaultRepository.GetAsync(pv => pv.Id == vaultId && pv.UserId == userId);
        if (vault == null)
        {
            throw new BusinessException("Password vault not found.");
        }
    }

    public void ValidateCurrentPassword(string storedPasswordHash, string providedPassword)
    {
        // This would typically use your hashing service to verify
        if (storedPasswordHash != providedPassword) // Simplified check
        {
            throw new BusinessException("Current password is incorrect.");
        }
    }
}
