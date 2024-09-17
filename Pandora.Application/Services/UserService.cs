using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pandora.Application.DTOs.UserDTOs;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Interfaces;
using Pandora.Application.Security.Interfaces;
using Pandora.Application.Utilities.Results.Implementations;
using Pandora.Application.Utilities.Results.Interfaces;
using Pandora.Application.Utilities.Results;
using Pandora.Core.Domain.Constants.Enums;
using Pandora.Core.Domain.Entities;
using System.Security.Authentication;
using FluentValidation;
using Pandora.Application.BusinessRules;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;
    private readonly IEncryption _encryption;
    private readonly IMapper _mapper;
    private readonly UserBusinessRules _userBusinessRules;
    private readonly IValidator<UserRegisterDto> _validator;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IHasher hasher, IEncryption encryption, IMapper mapper, UserBusinessRules userBusinessRules, IValidator<UserRegisterDto> validator, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _hasher = hasher;
        _encryption = encryption;
        _mapper = mapper;
        _userBusinessRules = userBusinessRules;
        _validator = validator;
        _logger = logger;
    }

    public async Task<UserDto> GetByEmailAsync(string email)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Kullanıcı bulunamadı: {Email}", email);
                return null;
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get user by email {Email}. Details: {ExceptionMessage}", nameof(GetByEmailAsync), email, ex.Message);
            throw new Exception("Kullanıcı bilgisi alınırken hata oluştu.", ex);
        }
    }

    public async Task<UserDto> GetByUsernameAsync(string username)
    {
        try
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
            {
                _logger.LogWarning("Kullanıcı bulunamadı: {Username}", username);
                return null;
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get user by username {Username}. Details: {ExceptionMessage}", nameof(GetByUsernameAsync), username, ex.Message);
            throw new Exception("Kullanıcı bilgisi alınırken hata oluştu.", ex);
        }
    }

    public async Task<IDataResult<UserDto>> RegisterUserAsync(UserRegisterDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return new DataResult<UserDto>(ResultStatus.Error, "Doğrulama hatası: " +
                string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), null);
        }

        await _userBusinessRules.UserNameCannotBeDuplicatedWhenInserted(dto.Username);
        await _userBusinessRules.EmailCannotBeDuplicatedWhenInserted(dto.Email);

        User user;
        if (dto.UserType == UserType.Individual)
            user = _mapper.Map<IndividualUser>(dto);
        else if (dto.UserType == UserType.Corporate)
            user = _mapper.Map<CorporateUser>(dto);
        else
            return new DataResult<UserDto>(ResultStatus.Error, "Geçersiz kullanıcı türü", null);

        user.NormalizedUsername = dto.Username.ToUpperInvariant();
        user.NormalizedEmail = dto.Email.ToUpperInvariant();
        user.SecurityStamp = Guid.NewGuid().ToString();
        user.PasswordHash = _hasher.HashPassword(dto.Password, HashAlgorithmType.Sha512);

        await _userRepository.AddAsync(user);

        return new DataResult<UserDto>(ResultStatus.Success, "Kullanıcı başarıyla kaydedildi.", _mapper.Map<UserDto>(user));
    }


    public async Task<IDataResult<UserDto>> UpdateUserAsync(UserUpdateDto dto)
    {
        try
        {
            var user = await _userRepository.GetAsync(u => u.Id == dto.Id);
            if (user == null)
            {
                return new DataResult<UserDto>(ResultStatus.Error, "Kullanıcı bulunamadı.", null);
            }

            _mapper.Map(dto, user);
            await _userRepository.UpdateAsync(user);
            var updatedUserDto = _mapper.Map<UserDto>(user);

            return new DataResult<UserDto>(ResultStatus.Success, "Kullanıcı başarıyla güncellendi.", updatedUserDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to update user. Details: {ExceptionMessage}", nameof(UpdateUserAsync), ex.Message);
            return new DataResult<UserDto>(ResultStatus.Error, "Kullanıcı güncellenirken hata oluştu.", data: null, ex);
        }
    }

    public Task<string> GeneratePasswordHashAsync(string password)
    {
        return Task.FromResult(_hasher.HashPassword(password, HashAlgorithmType.Sha512));
    }

    public Task<bool> VerifyPasswordHashAsync(string hashedPassword, string password)
    {
        var result = _hasher.VerifyHashedPassword(hashedPassword, password, HashAlgorithmType.Sha512);
        return Task.FromResult(result);
    }
}
