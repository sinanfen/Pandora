using Pandora.Application.Interfaces.Results;
using Pandora.Shared.DTOs.AuthDTOs;
using Pandora.Shared.DTOs.UserDTOs;
using System.Security.Claims;

namespace Pandora.Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Authenticate user and return tokens
    /// </summary>
    Task<IDataResult<TokenDto>> LoginAsync(UserLoginDto dto, string ipAddress, string userAgent, CancellationToken cancellationToken);
    
    /// <summary>
    /// Generate new access token using refresh token
    /// </summary>
    Task<IDataResult<TokenDto>> RefreshTokenAsync(RefreshTokenDto dto, string ipAddress, string userAgent, CancellationToken cancellationToken);
    
    /// <summary>
    /// Revoke a refresh token
    /// </summary>
    Task<IResult> RevokeTokenAsync(RevokeTokenDto dto, CancellationToken cancellationToken);
    
    /// <summary>
    /// Logout user and revoke all tokens
    /// </summary>
    Task<IResult> LogoutAsync(Guid userId, CancellationToken cancellationToken);
    
    /// <summary>
    /// Get user's active sessions
    /// </summary>
    Task<IDataResult<List<SessionDto>>> GetActiveSessionsAsync(Guid userId, string currentRefreshToken, CancellationToken cancellationToken);
    
    /// <summary>
    /// Revoke all sessions except current
    /// </summary>
    Task<IResult> RevokeAllOtherSessionsAsync(Guid userId, string currentRefreshToken, CancellationToken cancellationToken);
    
    /// <summary>
    /// Generate JWT token (legacy method)
    /// </summary>
    string GenerateToken(UserDto user, List<string> roles);
    
    /// <summary>
    /// Validate expired token (legacy method)
    /// </summary>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    
    /// <summary>
    /// Verify password (legacy method)
    /// </summary>
    bool VerifyPassword(string hashedPassword, string plainPassword);
    
    /// <summary>
    /// Send email verification token to user
    /// </summary>
    Task<IResult> SendEmailVerificationAsync(string email, string ipAddress, string userAgent, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Verify email with provided token
    /// </summary>
    Task<IDataResult<EmailVerificationResponseDto>> VerifyEmailAsync(EmailVerificationDto emailVerificationDto, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Resend email verification for existing user
    /// </summary>
    Task<IResult> ResendEmailVerificationAsync(ResendEmailVerificationDto resendEmailDto, string ipAddress, string userAgent, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Check if user's email is verified
    /// </summary>
    Task<IDataResult<bool>> IsEmailVerifiedAsync(string email, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Change user's email (requires current password)
    /// </summary>
    Task<IResult> ChangeEmailAsync(Guid userId, ChangeEmailDto changeEmailDto, string ipAddress, string userAgent, CancellationToken cancellationToken = default);
}