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

    // Kullanıcı adı benzersiz mi kontrol et
    public async Task UserNameCannotBeDuplicatedWhenInserted(string username)
    {
        var exists = await _userRepository.AnyAsync(u => u.NormalizedUsername == username.ToUpperInvariant());
        if (exists)
        {
            throw new BusinessException("Bu kullanıcı adı zaten alınmış.");
        }
    }

    // Email benzersiz mi kontrol et
    public async Task EmailCannotBeDuplicatedWhenInserted(string email)
    {
        var exists = await _userRepository.AnyAsync(u => u.NormalizedEmail == email.ToUpperInvariant());
        if (exists)
        {
            throw new BusinessException("Bu email zaten kayıtlı.");
        }
    }
}

