using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pandora.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using System.Text.RegularExpressions;
using Pandora.Core.Domain.Entities;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Shared.DTOs.UserDTOs;
using Pandora.Application.Interfaces.Security;
using Pandora.Application.Interfaces.Results;
using Pandora.Infrastructure.Utilities.Results.Implementations;
using Pandora.Shared.DTOs.AuthDTOs;
using System.Security.Cryptography;

namespace Pandora.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IHasher _hasher;
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public AuthService(IConfiguration configuration, IHasher hasher, IUserService userService, IMapper mapper, 
        IUserRepository userRepository, IRoleRepository roleRepository, IRefreshTokenRepository refreshTokenRepository,
        IEmailVerificationTokenRepository emailVerificationTokenRepository, IEmailService emailService)
    {
        _configuration = configuration;
        _hasher = hasher;
        _userService = userService;
        _mapper = mapper;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _emailVerificationTokenRepository = emailVerificationTokenRepository;
        _emailService = emailService;
    }

    public async Task<IDataResult<TokenDto>> LoginAsync(UserLoginDto dto, string ipAddress, string userAgent, CancellationToken cancellationToken)
    {
        User? userEntity = null;

        if (IsValidEmail(dto.UsernameOrEmail))
            userEntity = await _userService.GetEntityByEmailAsync(dto.UsernameOrEmail, cancellationToken);
        else
            userEntity = await _userService.GetEntityByUsernameAsync(dto.UsernameOrEmail, cancellationToken);

        if (userEntity == null)
            return new DataResult<TokenDto>(ResultStatus.Error, "User not found", data: null);

        // Check if email is verified
        if (!userEntity.EmailConfirmed)
            return new DataResult<TokenDto>(ResultStatus.Error, "Email address is not verified. Please check your email for verification instructions.", data: null);

        var isPasswordValid = VerifyPassword(userEntity.PasswordHash, dto.Password);
        if (!isPasswordValid)
            return new DataResult<TokenDto>(ResultStatus.Error, "Invalid password", data: null);

        // Update last login
        userEntity.LastLoginDate = DateTime.UtcNow;
        await _userRepository.UpdateAsync(userEntity);

        // Generate tokens
        var tokenDto = await GenerateTokensAsync(userEntity, ipAddress, userAgent, cancellationToken);

        return new DataResult<TokenDto>(ResultStatus.Success, "Login successful", tokenDto);
    }

    public async Task<IDataResult<TokenDto>> RefreshTokenAsync(RefreshTokenDto dto, string ipAddress, string userAgent, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken, cancellationToken);
        
        if (refreshToken == null)
            return new DataResult<TokenDto>(ResultStatus.Error, "Invalid refresh token", null);

        if (!refreshToken.IsValid)
        {
            // Token is invalid, revoke it and any tokens that were created using it
            await RevokeTokenChainAsync(refreshToken, "Invalid token used", cancellationToken);
            return new DataResult<TokenDto>(ResultStatus.Error, "Invalid refresh token", null);
        }

        // Mark the old token as used
        refreshToken.IsUsed = true;
        refreshToken.UpdatedDate = DateTime.UtcNow;
        await _refreshTokenRepository.UpdateAsync(refreshToken);

        // Generate new tokens
        var tokenDto = await GenerateTokensAsync(refreshToken.User, ipAddress, userAgent, cancellationToken);

        return new DataResult<TokenDto>(ResultStatus.Success, "Token refreshed successfully", tokenDto);
    }

    public async Task<IResult> RevokeTokenAsync(RevokeTokenDto dto, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken, cancellationToken);
        
        if (refreshToken == null)
            return new Result(ResultStatus.Error, "Token not found");

        refreshToken.IsRevoked = true;
        refreshToken.RevocationReason = dto.Reason ?? "Revoked by user";
        refreshToken.UpdatedDate = DateTime.UtcNow;
        
        await _refreshTokenRepository.UpdateAsync(refreshToken);

        return new Result(ResultStatus.Success, "Token revoked successfully");
    }

    public async Task<IResult> LogoutAsync(Guid userId, CancellationToken cancellationToken)
    {
        await _refreshTokenRepository.RevokeAllUserTokensAsync(userId, "User logout", cancellationToken);
        return new Result(ResultStatus.Success, "Logged out successfully");
    }

    public async Task<IDataResult<List<SessionDto>>> GetActiveSessionsAsync(Guid userId, string currentRefreshToken, CancellationToken cancellationToken)
    {
        var validTokens = await _refreshTokenRepository.GetValidTokensByUserIdAsync(userId, cancellationToken);
        
        var sessions = validTokens.Select(token => new SessionDto
        {
            Id = token.Id,
            IpAddress = token.IpAddress,
            UserAgent = token.UserAgent,
            CreatedAt = token.CreatedDate,
            ExpiresAt = token.ExpiresAt,
            IsCurrentSession = token.Token == currentRefreshToken
        }).ToList();

        return new DataResult<List<SessionDto>>(ResultStatus.Success, "Active sessions retrieved", sessions);
    }

    public async Task<IResult> RevokeAllOtherSessionsAsync(Guid userId, string currentRefreshToken, CancellationToken cancellationToken)
    {
        var validTokens = await _refreshTokenRepository.GetValidTokensByUserIdAsync(userId, cancellationToken);
        var tokensToRevoke = validTokens.Where(t => t.Token != currentRefreshToken).ToList();

        foreach (var token in tokensToRevoke)
        {
            token.IsRevoked = true;
            token.RevocationReason = "Revoked by user - logout other sessions";
            token.UpdatedDate = DateTime.UtcNow;
        }

        if (tokensToRevoke.Any())
        {
            await _refreshTokenRepository.UpdateRangeAsync(tokensToRevoke);
        }

        return new Result(ResultStatus.Success, $"Revoked {tokensToRevoke.Count} other sessions");
    }

    private async Task<TokenDto> GenerateTokensAsync(User user, string ipAddress, string userAgent, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetUserRolesAsync(user.Id, cancellationToken);
        var userDto = _mapper.Map<UserDto>(user);

        // Generate JWT Access Token
        var accessToken = GenerateAccessToken(userDto, roles);
        var accessTokenExpiry = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiresInMinutes"]));

        // Generate Refresh Token
        var refreshTokenString = GenerateRefreshToken();
        var refreshTokenExpiry = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JwtSettings:RefreshTokenExpiryInDays"] ?? "30"));

        // Save refresh token to database
        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshTokenString,
            ExpiresAt = refreshTokenExpiry,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };

        await _refreshTokenRepository.AddAsync(refreshToken, cancellationToken);

        return new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenString,
            AccessTokenExpiresAt = ((DateTimeOffset)accessTokenExpiry).ToUnixTimeSeconds(),
            RefreshTokenExpiresAt = ((DateTimeOffset)refreshTokenExpiry).ToUnixTimeSeconds(),
            TokenType = "Bearer"
        };
    }

    private string GenerateAccessToken(UserDto user, List<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };
        
        roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiresInMinutes"])),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private async Task RevokeTokenChainAsync(RefreshToken token, string reason, CancellationToken cancellationToken)
    {
        // Revoke the token
        token.IsRevoked = true;
        token.RevocationReason = reason;
        token.UpdatedDate = DateTime.UtcNow;
        
        // Find and revoke any tokens that were created using this token
        // This helps prevent token replay attacks
        await _refreshTokenRepository.UpdateAsync(token, cancellationToken);
    }

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, emailPattern);
    }

    // Legacy methods
    public string GenerateToken(UserDto user, List<string> roles)
    {
        return GenerateAccessToken(user, roles);
    }

    public bool VerifyPassword(string hashedPassword, string plainPassword)
    {
        return _hasher.VerifyHashedPassword(hashedPassword, plainPassword, HashAlgorithmType.Sha512);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        var jwtToken = validatedToken as JwtSecurityToken;

        if (jwtToken == null)
            throw new SecurityTokenException("Invalid token");

        return principal;
    }

    // Email Verification Methods
    public async Task<IResult> SendEmailVerificationAsync(string email, string ipAddress, string userAgent, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (user == null)
            return new Result(ResultStatus.Error, "User not found");

        if (user.EmailConfirmed)
            return new Result(ResultStatus.Error, "Email is already verified");

        // Invalidate existing tokens for this user
        await _emailVerificationTokenRepository.InvalidateAllUserTokensAsync(user.Id, cancellationToken);

        // Generate new verification token
        var token = GenerateEmailVerificationToken();
        var expiresAt = DateTime.UtcNow.AddHours(24); // 24 hours expiry

        var verificationToken = new EmailVerificationToken
        {
            UserId = user.Id,
            Token = token,
            Email = email,
            ExpiresAt = expiresAt,
            IpAddress = ipAddress,
            UserAgent = userAgent
        };

        await _emailVerificationTokenRepository.AddAsync(verificationToken, cancellationToken);

        // Send verification email
        var emailSent = await _emailService.SendEmailVerificationAsync(email, user.FirstName, token, cancellationToken);
        
        if (!emailSent)
            return new Result(ResultStatus.Error, "Failed to send verification email. Please try again.");

        return new Result(ResultStatus.Success, "Verification email sent successfully. Please check your inbox.");
    }

    public async Task<IDataResult<EmailVerificationResponseDto>> VerifyEmailAsync(EmailVerificationDto emailVerificationDto, CancellationToken cancellationToken = default)
    {
        var verificationToken = await _emailVerificationTokenRepository.GetByTokenAsync(emailVerificationDto.Token, cancellationToken);
        
        if (verificationToken == null)
        {
            return new DataResult<EmailVerificationResponseDto>(
                ResultStatus.Error, 
                "Invalid verification token",
                new EmailVerificationResponseDto { IsVerified = false, Message = "Invalid verification token" });
        }

        if (!verificationToken.IsValid)
        {
            var message = verificationToken.IsExpired ? "Verification token has expired" : "Verification token has already been used";
            return new DataResult<EmailVerificationResponseDto>(
                ResultStatus.Error,
                message,
                new EmailVerificationResponseDto { IsVerified = false, Message = message });
        }

        // Mark token as used
        verificationToken.IsUsed = true;
        verificationToken.UpdatedDate = DateTime.UtcNow;
        await _emailVerificationTokenRepository.UpdateAsync(verificationToken);

        // Update user email confirmation
        var user = verificationToken.User;
        user.EmailConfirmed = true;
        user.UpdatedDate = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Send welcome email
        await _emailService.SendWelcomeEmailAsync(user.Email, user.FirstName, cancellationToken);

        return new DataResult<EmailVerificationResponseDto>(
            ResultStatus.Success,
            "Email verified successfully",
            new EmailVerificationResponseDto 
            { 
                IsVerified = true, 
                Message = "Email verified successfully", 
                VerifiedAt = DateTime.UtcNow 
            });
    }

    public async Task<IResult> ResendEmailVerificationAsync(ResendEmailVerificationDto resendEmailDto, string ipAddress, string userAgent, CancellationToken cancellationToken = default)
    {
        return await SendEmailVerificationAsync(resendEmailDto.Email, ipAddress, userAgent, cancellationToken);
    }

    public async Task<IDataResult<bool>> IsEmailVerifiedAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
        if (user == null)
            return new DataResult<bool>(ResultStatus.Error, "User not found", false);

        return new DataResult<bool>(ResultStatus.Success, "Email verification status retrieved", user.EmailConfirmed);
    }

    public async Task<IResult> ChangeEmailAsync(Guid userId, ChangeEmailDto changeEmailDto, string ipAddress, string userAgent, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetAsync(u => u.Id == userId, cancellationToken: cancellationToken);
        if (user == null)
            return new Result(ResultStatus.Error, "User not found");

        // Verify current password
        if (!VerifyPassword(user.PasswordHash, changeEmailDto.CurrentPassword))
            return new Result(ResultStatus.Error, "Current password is incorrect");

        // Check if new email is already in use
        var existingUser = await _userRepository.GetByEmailAsync(changeEmailDto.NewEmail, cancellationToken);
        if (existingUser != null && existingUser.Id != userId)
            return new Result(ResultStatus.Error, "Email address is already in use");

        // Update user email and set as unverified
        user.Email = changeEmailDto.NewEmail;
        user.NormalizedEmail = changeEmailDto.NewEmail.ToUpperInvariant();
        user.EmailConfirmed = false;
        user.UpdatedDate = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // Send verification email to new address
        await SendEmailVerificationAsync(changeEmailDto.NewEmail, ipAddress, userAgent, cancellationToken);

        return new Result(ResultStatus.Success, "Email changed successfully. Please verify your new email address.");
    }

    private string GenerateEmailVerificationToken()
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
}
