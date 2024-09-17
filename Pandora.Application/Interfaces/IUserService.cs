using Pandora.Application.DTOs.UserDTOs;
using Pandora.Application.Utilities.Results.Interfaces;

namespace Pandora.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByEmailAsync(string email);          
    Task<UserDto?> GetByUsernameAsync(string username);    
    Task<IDataResult<UserDto>> RegisterUserAsync(UserRegisterDto dto);  
    Task<IDataResult<UserDto>> UpdateUserAsync(UserUpdateDto dto);      
    Task<string> GeneratePasswordHashAsync(string password);
    Task<bool> VerifyPasswordHashAsync(string hashedPassword, string password);
}
