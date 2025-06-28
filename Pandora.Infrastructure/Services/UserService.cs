﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Interfaces;
using Pandora.Core.Domain.Entities;
using System.Security.Authentication;
using FluentValidation;
using Pandora.Application.BusinessRules;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Pandora.Core.Persistence.Paging;
using Pandora.Core.Domain.Paging;
using Pandora.Shared.DTOs.UserDTOs;
using Pandora.Application.Interfaces.Security;
using Pandora.Application.Interfaces.Results;
using Pandora.Infrastructure.Utilities.Results.Implementations;

namespace Pandora.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IHasher _hasher;
    private readonly IEncryption _encryption;
    private readonly IMapper _mapper;
    private readonly UserBusinessRules _userBusinessRules;
    private readonly IValidator<UserRegisterDto> _userRegisterDtoValidator;
    private readonly IValidator<UserUpdateDto> _userUpdateDtoValidator;
    private readonly IValidator<UserPasswordChangeDto> _userPasswordChangeDtoValidator;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IHasher hasher, IEncryption encryption, IMapper mapper, UserBusinessRules userBusinessRules,
        ILogger<UserService> logger, IValidator<UserRegisterDto> userRegisterDtoValidator, IValidator<UserUpdateDto> userUpdateDtoValidator,
        IValidator<UserPasswordChangeDto> userPasswordChangeDtoValidator)
    {
        _userRepository = userRepository;
        _hasher = hasher;
        _encryption = encryption;
        _mapper = mapper;
        _userBusinessRules = userBusinessRules;
        _logger = logger;
        _userRegisterDtoValidator = userRegisterDtoValidator;
        _userUpdateDtoValidator = userUpdateDtoValidator;
        _userPasswordChangeDtoValidator = userPasswordChangeDtoValidator;
    }

    public async Task<UserDto?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
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

    public async Task<UserDto?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetByUsernameAsync(username, cancellationToken);
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

    public async Task<User?> GetEntityByEmailAsync(string email, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetAsync(u => u.NormalizedEmail == email.ToUpperInvariant(), cancellationToken: cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User not found: {Email}", email);
                return null;
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get user information. Details: {ExceptionMessage}", nameof(GetEntityByEmailAsync), ex.Message);
            throw new Exception("Error occurred while retrieving user information.", ex);
        }
    }

    public async Task<User?> GetEntityByUsernameAsync(string username, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetAsync(u => u.NormalizedUsername == username.ToUpperInvariant(), cancellationToken: cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", username);
                return null;
            }
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get user by username {Username}. Details: {ExceptionMessage}", nameof(GetEntityByUsernameAsync), username, ex.Message);
            throw new Exception("Error occurred while retrieving user information.", ex);
        }
    }

    public async Task<IDataResult<UserDto>> RegisterUserAsync(UserRegisterDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _userRegisterDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return new DataResult<UserDto>(ResultStatus.Error, "Validation Errors: " +
                    string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), null);
            }

            await _userBusinessRules.UsernameCannotBeDuplicatedWhenRegistering(dto.Username);
            await _userBusinessRules.EmailCannotBeDuplicatedWhenRegistering(dto.Email);

            var user = _mapper.Map<User>(dto);

            user.NormalizedUsername = dto.Username.ToUpperInvariant();
            user.NormalizedEmail = dto.Email.ToUpperInvariant();
            user.SecurityStamp = Guid.NewGuid().ToString();
            user.EmailConfirmed = false; // Email verification required

            user.PasswordHash = _hasher.HashPassword(dto.Password, HashAlgorithmType.Sha512);

            await _userRepository.AddAsync(user, cancellationToken);

            // Başarılı sonuç - Email verification will be handled separately
            return new DataResult<UserDto>(ResultStatus.Success, 
                "User successfully registered. Please verify your email address to complete registration.", 
                _mapper.Map<UserDto>(user));
        }
        catch (Exception ex)
        {
            // Hata durumunda loglama ve hata sonucu
            _logger.LogError(ex, "Error in {MethodName}. Failed to register the user. Details: {ExceptionMessage}", nameof(RegisterUserAsync), ex.Message);
            return new DataResult<UserDto>(ResultStatus.Error, ex.Message, null);
        }
    }

    public async Task<IDataResult<UserDto>> UpdateAsync(UserUpdateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _userUpdateDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return new DataResult<UserDto>(ResultStatus.Error, "Validation failed", null);

            var user = await _userRepository.GetAsync(u => u.Id == dto.Id);
            if (user == null)
                return new DataResult<UserDto>(ResultStatus.Error, "User not found", null);

            _mapper.Map(dto, user);

            await _userRepository.UpdateAsync(user, cancellationToken);

            var updatedUserDto = _mapper.Map<UserDto>(user);
            return new DataResult<UserDto>(ResultStatus.Success, "User updated successfully", updatedUserDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to update the user. Details: {ExceptionMessage}", nameof(UpdateAsync), ex.Message);
            return new DataResult<UserDto>(ResultStatus.Error, ex.Message, null);
        }
    }

    public async Task<Pandora.Application.Interfaces.Results.IResult> DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            User? user = await _userRepository.GetAsync(x => x.Id == userId, cancellationToken: cancellationToken);
            if (user == null)
            {
                return new Result(ResultStatus.Warning, "User not found.");
            }
            await _userRepository.DeleteAsync(user, cancellationToken: cancellationToken);
            return new Result(ResultStatus.Success, "User successfully deleted.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get user. Details: {ExceptionMessage}", nameof(GetByIdAsync), ex.Message);
            return new Result(ResultStatus.Error, "Failed to get user.", ex);
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

    public async Task<UserDto?> GetAsync(Expression<Func<User, bool>> predicate, Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        try
        {
            User? user = await _userRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get user. Details: {ExceptionMessage}", nameof(GetAsync), ex.Message);
            throw;
        }
    }

    public async Task<Paginate<UserDto>?> GetListAsync(Expression<Func<User, bool>>? predicate = null, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null, Func<IQueryable<User>, IIncludableQueryable<User, object>>? include = null, int index = 0, int size = 10, bool withDeleted = false, bool enableTracking = true, CancellationToken cancellationToken = default)
    {
        try
        {
            IPaginate<User> userList = await _userRepository.GetListAsync(predicate, orderBy, include, index, size, withDeleted, enableTracking, cancellationToken);
            return _mapper.Map<Paginate<UserDto>>(userList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get paged list of user. Details: {ExceptionMessage}", nameof(GetListAsync), ex.Message);
            throw;
        }
    }

    public async Task<UserDto> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userRepository.GetAsync(x => x.Id == userId, cancellationToken: cancellationToken);

            if (user == null)
            {
                _logger.LogWarning("User not found with ID: {UserId}", userId);
                return null;
            }

            return _mapper.Map<UserDto>(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get user. Details: {ExceptionMessage}", nameof(GetByIdAsync), ex.Message);
            throw;
        }
    }

    public async Task<List<UserDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false)
    {
        try
        {
            var pagedData = await _userRepository.GetListAsync(cancellationToken: cancellationToken);
            var userDtos = _mapper.Map<List<UserDto>>(pagedData.Items);
            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get list of user. Details: {ExceptionMessage}", nameof(GetAllAsync), ex.Message);
            throw;
        }
    }

    public async Task<Pandora.Application.Interfaces.Results.IResult> ChangePasswordAsync(UserPasswordChangeDto userPasswordChangeDto, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _userPasswordChangeDtoValidator.ValidateAsync(userPasswordChangeDto);
            if (!validationResult.IsValid)
            {
                return new Result(ResultStatus.Error, "Validation error: " + string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }

            var user = await _userRepository.GetAsync(u => u.Id == userPasswordChangeDto.Id, cancellationToken: cancellationToken);
            if (user == null)
            {
                return new Result(ResultStatus.Error, "User not found.");
            }

            if (!_hasher.VerifyHashedPassword(user.PasswordHash, userPasswordChangeDto.CurrentPassword, HashAlgorithmType.Sha512))
            {
                return new Result(ResultStatus.Error, "Current password is invalid.");
            }

            // Hash new password and update
            user.PasswordHash = _hasher.HashPassword(userPasswordChangeDto.NewPassword, HashAlgorithmType.Sha512);
            user.LastPasswordChangeDate = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user, cancellationToken);

            return new Result(ResultStatus.Success, "Password successfully changed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to change password. Details: {ExceptionMessage}", nameof(ChangePasswordAsync), ex.Message);
            return new Result(ResultStatus.Error, "An error occurred while changing the password.");
        }
    }
}
