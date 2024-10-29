using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Security.Interfaces;
using Pandora.CrossCuttingConcerns.ExceptionHandling;
using System.Security.Authentication;

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

    public void EnsurePasswordMeetsComplexityRules(string password)
    {
        if (password.Length < 8 || !password.Any(char.IsDigit) || !password.Any(char.IsUpper))
        {
            throw new BusinessException("Şifre en az 8 karakter uzunluğunda, bir büyük harf ve bir rakam içermelidir.");
        }
    }

    public void EnsurePasswordChangeIsRecent(DateTime? lastPasswordChangeDate)
    {
        if (lastPasswordChangeDate.HasValue && (DateTime.UtcNow - lastPasswordChangeDate.Value).Days > 90)
        {
            throw new BusinessException("Bu şifre 90 günden fazla bir süredir değiştirilmedi.");
        }
    }

    public void EnsurePasswordNotExpired(DateTime? passwordExpirationDate)
    {
        if (passwordExpirationDate.HasValue && passwordExpirationDate.Value < DateTime.UtcNow)
        {
            throw new BusinessException("Bu şifrenin geçerlilik süresi dolmuş.");
        }
    }

    public async Task EnsurePasswordHashIsUnique(string passwordHash)
    {
        var exists = await _passwordVaultRepository.AnyAsync(pv => pv.PasswordHash == passwordHash);
        if (exists)
        {
            throw new BusinessException("Bu şifre daha önce kullanılmış.");
        }
    }

    public async Task CheckCurrentPasswordAsync(Guid passwordVaultId, string currentPassword)
    {
        var passwordVault = await _passwordVaultRepository.GetAsync(pv => pv.Id == passwordVaultId);

        if (passwordVault == null)
            throw new BusinessException("Şifre kasası bulunamadı.");

        // AES ile şifrelenmiş şifreyi çöz ve karşılaştır
        //var decryptedPassword = _encryption.Decrypt(passwordVault.PasswordHash);

        if (!passwordVault.PasswordHash.Equals(currentPassword))
            throw new BusinessException("Mevcut şifre doğru değil.");
    }

}
