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

namespace Pandora.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IHasher _hasher;
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public AuthService(IConfiguration configuration, IHasher hasher, IUserService userService, IMapper mapper, IUserRepository userRepository,
        IRoleRepository roleRepository)
    {
        _configuration = configuration;
        _hasher = hasher;
        _userService = userService;
        _mapper = mapper;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    // JWT Token üretme
    public string GenerateToken(UserDto user, List<string> roles)
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
            Issuer = _configuration["JwtSettings:Issuer"], // Issuer (token'ı oluşturan taraf)
            Audience = _configuration["JwtSettings:Audience"], // Audience (token'ı kullanacak taraf)
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    // Kullanıcı girişi (Login)
    public async Task<IDataResult<string>> LoginAsync(UserLoginDto dto, CancellationToken cancellationToken)
    {
        User? userEntity = null;

        if (IsValidEmail(dto.UsernameOrEmail))
            userEntity = await _userService.GetEntityByEmailAsync(dto.UsernameOrEmail, cancellationToken);
        else
            userEntity = await _userService.GetEntityByUsernameAsync(dto.UsernameOrEmail, cancellationToken);

        if (userEntity == null)
            return new DataResult<string>(ResultStatus.Error, "Kullanıcı bulunamadı", data: null);

        var isPasswordValid = VerifyPassword(userEntity.PasswordHash, dto.Password);
        if (!isPasswordValid)
            return new DataResult<string>(ResultStatus.Error, "Geçersiz şifre", data: null);

        userEntity.LastLoginDate = DateTime.UtcNow;
        await _userRepository.UpdateAsync(userEntity);
        var roles = await _roleRepository.GetUserRolesAsync(userEntity.Id, cancellationToken);
        var userDto = _mapper.Map<UserDto>(userEntity);
        var token = GenerateToken(userDto, roles);

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
