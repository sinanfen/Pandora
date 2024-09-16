using Pandora.Application.DTOs.UserDTOs;

namespace Pandora.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByEmailAsync(string email);          
    Task<UserDto?> GetByUsernameAsync(string username);    
    Task<UserDto> RegisterUserAsync(UserRegisterDto dto);  
    Task<UserDto> UpdateUserAsync(UserUpdateDto dto);      
    Task<string> GeneratePasswordHashAsync(string password);
    Task<bool> VerifyPasswordHashAsync(string hashedPassword, string password);
}
