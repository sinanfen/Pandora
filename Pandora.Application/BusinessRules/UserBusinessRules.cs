using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Security.Interfaces;
using Pandora.CrossCuttingConcerns.ExceptionHandling;
using System.Security.Authentication;

namespace Pandora.Application.BusinessRules;

public class UserBusinessRules
{
    private readonly IUserRepository _userRepository;

    public UserBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Kullanıcı adı benzersiz mi kontrol et (insert sırasında)
    public async Task UserNameCannotBeDuplicatedWhenInserted(string username)
    {
        var exists = await _userRepository.AnyAsync(u => u.NormalizedUsername == username.ToUpperInvariant());
        if (exists)
        {
            throw new BusinessException("Bu kullanıcı adı zaten alınmış.");
        }
    }

    // Email benzersiz mi kontrol et (insert sırasında)
    public async Task EmailCannotBeDuplicatedWhenInserted(string email)
    {
        var exists = await _userRepository.AnyAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
        if (exists)
        {
            throw new BusinessException("Bu email zaten kayıtlı.");
        }
    }

    // Kullanıcı adı benzersiz mi kontrol et (update sırasında)
    public async Task UserNameCannotBeDuplicatedWhenUpdated(Guid userId, string username)
    {
        var exists = await _userRepository.AnyAsync(u => u.NormalizedUsername == username.ToUpperInvariant() && u.Id != userId);
        if (exists)
        {
            throw new BusinessException("Bu kullanıcı adı zaten alınmış.");
        }
    }

    // Email benzersiz mi kontrol et (update sırasında)
    public async Task EmailCannotBeDuplicatedWhenUpdated(Guid userId, string email)
    {
        var exists = await _userRepository.AnyAsync(u => u.NormalizedEmail == email.ToUpperInvariant() && u.Id != userId);
        if (exists)
        {
            throw new BusinessException("Bu email zaten kayıtlı.");
        }
    }

    // Password complexity rule
    public void EnsurePasswordMeetsComplexityRules(string password)
    {
        var specialCharacters = "!@#$%^&*()_-+=<>?{}[]|\\/~`.,;:'\"";

        if (password.Length < 8 ||
            !password.Any(char.IsDigit) ||
            !password.Any(char.IsUpper) ||
            !password.Any(c => specialCharacters.Contains(c)))
        {
            throw new BusinessException("Şifre en az 8 karakter uzunluğunda, bir büyük harf, bir rakam ve bir özel karakter içermelidir.");
        }
    }


    // Ensure the current password is correct
    public bool EnsureCurrentPasswordIsCorrect(string currentPassword, string hashedPassword, IHasher hasher)
    {
        return hasher.VerifyHashedPassword(hashedPassword, currentPassword, HashAlgorithmType.Sha512);
    }
}


