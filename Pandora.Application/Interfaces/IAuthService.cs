using Pandora.Application.DTOs.UserDTOs;
using Pandora.Application.Utilities.Results.Interfaces;
using System.Security.Claims;

namespace Pandora.Application.Interfaces;

public interface IAuthService
{
    string GenerateToken(UserDto user);
    Task<IDataResult<string>> LoginAsync(UserLoginDto dto,CancellationToken cancellationToken);
    bool VerifyPassword(string hashedPassword, string plainPassword);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}