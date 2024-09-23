using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pandora.Application.DTOs.UserDTOs;
using Pandora.Application.Interfaces;
using Pandora.Application.Security.Interfaces;
using Pandora.Application.Utilities.Results.Implementations;
using Pandora.Application.Utilities.Results.Interfaces;
using Pandora.Application.Utilities.Results;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using System.Text.RegularExpressions;
using Pandora.Core.Domain.Entities;

namespace Pandora.Application.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IHasher _hasher;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public AuthService(IConfiguration configuration, IHasher hasher, IUserService userService, IMapper mapper)
    {
        _configuration = configuration;
        _hasher = hasher;
        _userService = userService;
        _mapper = mapper;
    }

    // JWT Token üretme
    public string GenerateToken(UserDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.UserType.ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiresInMinutes"])),
            Issuer = _configuration["JwtSettings:Issuer"], // Issuer (token'ı oluşturan taraf)
            Audience = _configuration["JwtSettings:Audience"], // Audience (token'ı kullanacak taraf)
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // Kullanıcı girişi (Login)
    public async Task<IDataResult<string>> LoginAsync(UserLoginDto dto)
    {
        var cts = new CancellationTokenSource();
        User? userEntity = null;

        if (IsValidEmail(dto.UsernameOrEmail))
            userEntity = await _userService.GetEntityByEmailAsync(dto.UsernameOrEmail, cts.Token);
        else
            userEntity = await _userService.GetEntityByUsernameAsync(dto.UsernameOrEmail, cts.Token);

        if (userEntity == null)
            return new DataResult<string>(ResultStatus.Error, "Kullanıcı bulunamadı", data: null);

        var isPasswordValid = VerifyPassword(userEntity.PasswordHash, dto.Password);
        if (!isPasswordValid)
            return new DataResult<string>(ResultStatus.Error, "Geçersiz şifre", data: null);

        var userDto = _mapper.Map<UserDto>(userEntity);
        var token = GenerateToken(userDto);

        return new DataResult<string>(ResultStatus.Success, "Giriş başarılı", token);
    }

    private bool IsValidEmail(string email)
    {
        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$"; // Basit bir e-posta doğrulama deseni
        return Regex.IsMatch(email, emailPattern);
    }

    public bool VerifyPassword(string hashedPassword, string plainPassword)
    {
        return _hasher.VerifyHashedPassword(hashedPassword, plainPassword, HashAlgorithmType.Sha512);
    }

    // Token doğrulama (Token'ı decode edip kontrol etmek)
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
            ValidateLifetime = false, // Token'ın süresi dolmuş olabilir, bu yüzden zaman kontrolü yapılmaz
            ClockSkew = TimeSpan.Zero
        };

        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        var jwtToken = validatedToken as JwtSecurityToken;

        if (jwtToken == null)
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}
