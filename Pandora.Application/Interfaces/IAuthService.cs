using Pandora.Application.Utilities.Results.Interfaces;
using Pandora.Shared.DTOs.UserDTOs;
using System.Security.Claims;

namespace Pandora.Application.Interfaces;

public interface IAuthService
{
    string GenerateToken(UserDto user);
    Task<IDataResult<string>> LoginAsync(UserLoginDto dto,CancellationToken cancellationToken);
    bool VerifyPassword(string hashedPassword, string plainPassword);
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}