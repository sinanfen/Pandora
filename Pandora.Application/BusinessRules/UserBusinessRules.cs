using Pandora.Application.Interfaces.Repositories;
using Pandora.CrossCuttingConcerns.ExceptionHandling;

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
}


