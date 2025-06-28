using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Interfaces.Security;
using Pandora.CrossCuttingConcerns.ExceptionHandling;
using System.Security.Authentication;
using System.Text.RegularExpressions;

namespace Pandora.Application.BusinessRules;

public class UserBusinessRules
{
    private readonly IUserRepository _userRepository;

    public UserBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    // Check if username is unique during registration
    public async Task UsernameCannotBeDuplicatedWhenRegistering(string username)
    {
        var exists = await _userRepository.AnyAsync(u => u.NormalizedUsername == username.ToUpper());
        if (exists)
        {
            throw new BusinessException("This username is already taken.");
        }
    }

    // Check if email is unique during registration
    public async Task EmailCannotBeDuplicatedWhenRegistering(string email)
    {
        var exists = await _userRepository.AnyAsync(u => u.NormalizedEmail == email.ToUpper());
        if (exists)
        {
            throw new BusinessException("This email is already registered.");
        }
    }

    // Check if username is unique during update
    public async Task UsernameCannotBeDuplicatedWhenUpdating(Guid userId, string username)
    {
        var exists = await _userRepository.AnyAsync(u => u.NormalizedUsername == username.ToUpper() && u.Id != userId);
        if (exists)
        {
            throw new BusinessException("This username is already taken.");
        }
    }

    // Check if email is unique during update
    public async Task EmailCannotBeDuplicatedWhenUpdating(Guid userId, string email)
    {
        var exists = await _userRepository.AnyAsync(u => u.NormalizedEmail == email.ToUpper() && u.Id != userId);
        if (exists)
        {
            throw new BusinessException("This email is already registered.");
        }
    }

    // Password strength validation
    public void PasswordMustMeetComplexityRequirements(string password)
    {
        const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$";
        if (!Regex.IsMatch(password, pattern))
        {
            throw new BusinessException("Password must be at least 8 characters long and contain an uppercase letter, a digit, and a special character.");
        }
    }

    // Ensure the current password is correct
    public bool EnsureCurrentPasswordIsCorrect(string currentPassword, string hashedPassword, IHasher hasher)
    {
        return hasher.VerifyHashedPassword(hashedPassword, currentPassword, HashAlgorithmType.Sha512);
    }
}


