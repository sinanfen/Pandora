using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Security.Interfaces;
using Pandora.Application.Utilities.Results.Implementations;
using Pandora.Application.Utilities.Results;
using Pandora.Application.Utilities.Results.Interfaces;
using Pandora.Application.Validators.PasswordVaultValidators;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using System.Linq.Expressions;
using System.Security.Authentication;
using Microsoft.Extensions.Logging;
using Pandora.CrossCuttingConcerns.ExceptionHandling;
using Pandora.Shared.DTOs.PasswordVaultDTOs;

namespace Pandora.Application.Services;

public class PasswordVaultService : IPasswordVaultService
{
    private readonly IPasswordVaultRepository _passwordVaultRepository;
    private readonly PasswordVaultAddDtoValidator _passwordVaultAddDtoValidator;
    private readonly PasswordVaultUpdateDtoValidator _passwordVaultUpdateDtoValidator;
    private readonly PasswordVaultBusinessRules _passwordVaultBusinessRules;
    private readonly IMapper _mapper;
    private readonly IHasher _hasher;
    private readonly IEncryption _encryption;
    private readonly ILogger<PasswordVaultService> _logger;

    public PasswordVaultService(IPasswordVaultRepository passwordVaultRepository, IMapper mapper, IHasher hasher, IEncryption encryption,
        PasswordVaultAddDtoValidator passwordVaultAddDtoValidator, PasswordVaultUpdateDtoValidator passwordVaultUpdateDtoValidator,
        PasswordVaultBusinessRules passwordVaultBusinessRules, ILogger<PasswordVaultService> logger)
    {
        _passwordVaultRepository = passwordVaultRepository;
        _mapper = mapper;
        _hasher = hasher;
        _encryption = encryption;
        _passwordVaultAddDtoValidator = passwordVaultAddDtoValidator;
        _passwordVaultUpdateDtoValidator = passwordVaultUpdateDtoValidator;
        _passwordVaultBusinessRules = passwordVaultBusinessRules;
        _logger = logger;
    }

    public async Task<IDataResult<PasswordVaultDto>> AddAsync(PasswordVaultAddDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _passwordVaultAddDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return new DataResult<PasswordVaultDto>(ResultStatus.Error, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), null);

            _passwordVaultBusinessRules.EnsurePasswordMeetsComplexityRules(dto.Password);

            var passwordVault = _mapper.Map<PasswordVault>(dto);

            EncryptFields(passwordVault, dto);

            // Hash the password
            passwordVault.PasswordHash = _hasher.HashPassword(dto.Password, HashAlgorithmType.Sha512);

            await _passwordVaultRepository.AddAsync(passwordVault, cancellationToken);

            var resultDto = _mapper.Map<PasswordVaultDto>(passwordVault);
            return new DataResult<PasswordVaultDto>(ResultStatus.Success, "Password vault added successfully", resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to add password vault. Details: {ExceptionMessage}", nameof(AddAsync), ex.Message);
            return new DataResult<PasswordVaultDto>(ResultStatus.Error, "An error occurred while adding the password vault.", null);
        }
    }

    public async Task<IDataResult<PasswordVaultDto>> UpdateAsync(PasswordVaultUpdateDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _passwordVaultUpdateDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return new DataResult<PasswordVaultDto>(ResultStatus.Error, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), null);

            var passwordVault = await _passwordVaultRepository.GetAsync(x => x.Id == dto.Id, cancellationToken: cancellationToken);
            if (passwordVault == null)
                return new DataResult<PasswordVaultDto>(ResultStatus.Error, "Password vault not found.", null);

            await _passwordVaultBusinessRules.CheckCurrentPasswordAsync(dto.Id, dto.CurrentPassword);

            _passwordVaultBusinessRules.EnsurePasswordMeetsComplexityRules(dto.NewPassword);

            passwordVault.PasswordHash = _hasher.HashPassword(dto.NewPassword, HashAlgorithmType.Sha512);
            _mapper.Map(dto, passwordVault);

            EncryptFields(passwordVault, dto);
            await _passwordVaultRepository.UpdateAsync(passwordVault, cancellationToken);

            var resultDto = _mapper.Map<PasswordVaultDto>(passwordVault);
            return new DataResult<PasswordVaultDto>(ResultStatus.Success, "Password vault updated successfully", resultDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to update password vault. Details: {ExceptionMessage}", nameof(UpdateAsync), ex.Message);
            return new DataResult<PasswordVaultDto>(ResultStatus.Error, "An error occurred while updating the password vault.", null);
        }
    }

    public async Task<IResult> DeleteAsync(Guid passwordVaultId, CancellationToken cancellationToken)
    {
        var passwordVault = await _passwordVaultRepository.GetAsync(x => x.Id == passwordVaultId, cancellationToken: cancellationToken);
        if (passwordVault == null)
            return new Result(ResultStatus.Warning, "Password vault not found.");

        await _passwordVaultRepository.DeleteAsync(passwordVault, cancellationToken: cancellationToken);
        return new Result(ResultStatus.Success, "Password vault successfully deleted.");
    }

    public async Task<List<PasswordVaultDto>> GetAllAsync(CancellationToken cancellationToken, bool withDeleted = false)
    {
        try
        {
            var passwordVaults = await _passwordVaultRepository.GetListAsync(cancellationToken: cancellationToken);
            if (passwordVaults == null || !passwordVaults.Items.Any())
                return new List<PasswordVaultDto>();

            foreach (var vault in passwordVaults.Items)
                DecryptFields(vault);

            return _mapper.Map<List<PasswordVaultDto>>(passwordVaults.Items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get all password vaults. Details: {ExceptionMessage}", nameof(GetAllAsync), ex.Message);
            throw new BusinessException("An error occurred while fetching the password vaults.");
        }
    }

    public async Task<List<PasswordVaultDto>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken, bool withDeleted = false)
    {
        try
        {
            var passwordVaults = await _passwordVaultRepository.GetListAsync(x => x.UserId == userId, cancellationToken: cancellationToken);
            if (passwordVaults == null || !passwordVaults.Items.Any())
                return new List<PasswordVaultDto>();

            foreach (var vault in passwordVaults.Items)
                DecryptFields(vault);

            return _mapper.Map<List<PasswordVaultDto>>(passwordVaults.Items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get all password vaults. Details: {ExceptionMessage}", nameof(GetAllAsync), ex.Message);
            throw new BusinessException("An error occurred while fetching the password vaults.");
        }
    }

    public async Task<PasswordVaultDto?> GetAsync(
    Expression<Func<PasswordVault, bool>> predicate,
    Func<IQueryable<PasswordVault>, IIncludableQueryable<PasswordVault, object>>? include = null,
    bool withDeleted = false,
    bool enableTracking = true,
    CancellationToken cancellationToken = default)
    {
        try
        {
            var passwordVault = await _passwordVaultRepository.GetAsync(predicate, include, withDeleted, enableTracking, cancellationToken);
            if (passwordVault == null)
                return null;

            DecryptFields(passwordVault);
            return _mapper.Map<PasswordVaultDto>(passwordVault);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get password vault. Details: {ExceptionMessage}", nameof(GetAsync), ex.Message);
            throw new BusinessException("An error occurred while retrieving the password vault.");
        }
    }

    public async Task<PasswordVaultDto> GetByIdAndUserAsync(Guid passwordVaultId, Guid userId, CancellationToken cancellationToken)
    {
        var passwordVault = await _passwordVaultRepository.GetAsync(x => x.Id == passwordVaultId && x.UserId == userId, cancellationToken: cancellationToken);
        if (passwordVault == null)
            throw new BusinessException("Password vault not found.");

        DecryptFields(passwordVault);
        return _mapper.Map<PasswordVaultDto>(passwordVault);
    }

    public async Task<PasswordVaultDto> GetByIdAsync(Guid passwordVaultId, CancellationToken cancellationToken)
    {
        var passwordVault = await _passwordVaultRepository.GetAsync(x => x.Id == passwordVaultId, cancellationToken: cancellationToken);
        if (passwordVault == null)
            throw new BusinessException("Password vault not found.");

        DecryptFields(passwordVault);
        return _mapper.Map<PasswordVaultDto>(passwordVault);
    }

    public async Task<Paginate<PasswordVaultDto>?> GetListAsync(
    Expression<Func<PasswordVault, bool>>? predicate = null,
    Func<IQueryable<PasswordVault>, IOrderedQueryable<PasswordVault>>? orderBy = null,
    Func<IQueryable<PasswordVault>, IIncludableQueryable<PasswordVault, object>>? include = null,
    int index = 0,
    int size = 10,
    bool withDeleted = false,
    bool enableTracking = true,
    CancellationToken cancellationToken = default)
    {
        var passwordVaults = await _passwordVaultRepository.GetListAsync(predicate, orderBy, include, index, size, withDeleted, enableTracking, cancellationToken);
        foreach (var vault in passwordVaults.Items)
            DecryptFields(vault);

        return _mapper.Map<Paginate<PasswordVaultDto>>(passwordVaults);
    }

    private void EncryptFields<T>(PasswordVault passwordVault, T dto) where T : IPasswordVaultDto
    {
        passwordVault.SecureSiteName = _encryption.Encrypt(dto.SiteName);
        passwordVault.SecureNotes = _encryption.Encrypt(dto.Notes);
        passwordVault.SecureUsernameOrEmail = _encryption.Encrypt(dto.UsernameOrEmail);
    }

    private void DecryptFields(PasswordVault passwordVault)
    {
        passwordVault.SecureSiteName = _encryption.Decrypt(passwordVault.SecureSiteName);
        passwordVault.SecureNotes = _encryption.Decrypt(passwordVault.SecureNotes);
        passwordVault.SecureUsernameOrEmail = _encryption.Decrypt(passwordVault.SecureUsernameOrEmail);
    }
}