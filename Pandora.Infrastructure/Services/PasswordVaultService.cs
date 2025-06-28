using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Pandora.Application.BusinessRules;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Interfaces.Results;
using Pandora.Application.Interfaces.Security;
using Pandora.Application.Validators.PasswordVaultValidators;
using Pandora.Core.Domain.Entities;
using Pandora.Core.Persistence.Paging;
using Pandora.CrossCuttingConcerns.ExceptionHandling;
using Pandora.Infrastructure.Utilities.Results.Implementations;
using Pandora.Shared.DTOs.PasswordVaultDTOs;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace Pandora.Infrastructure.Services;

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

    public async Task<IDataResult<PasswordVaultDto>> AddAsync(PasswordVaultAddDto dto, Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _passwordVaultAddDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return new DataResult<PasswordVaultDto>(ResultStatus.Error, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), null);

            var passwordVault = _mapper.Map<PasswordVault>(dto);
            passwordVault.UserId = userId; // JWT token'dan gelen kullanıcı ID'sini set ediyoruz

            EncryptFields(passwordVault, dto);

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

    public async Task<IDataResult<PasswordVaultDto>> UpdateAsync(PasswordVaultUpdateDto dto, Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _passwordVaultUpdateDtoValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
                return new DataResult<PasswordVaultDto>(ResultStatus.Error, string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), null);

            var passwordVault = await _passwordVaultRepository.GetAsync(x => x.Id == dto.Id && x.UserId == userId, cancellationToken: cancellationToken);
            if (passwordVault == null)
                return new DataResult<PasswordVaultDto>(ResultStatus.Error, "Password vault not found or unauthorized.", null);

            // Mevcut şifreyi doğrula
            //await _passwordVaultBusinessRules.CheckCurrentPasswordAsync(dto.Id, dto.Password);

            // Yeni şifreyi AES ile şifrele ve kaydet
            passwordVault.PasswordHash = _encryption.Encrypt(dto.NewPassword);

            // Diğer alanları güncelle
            _mapper.Map(dto, passwordVault);
            passwordVault.UserId = userId; // JWT token'dan gelen kullanıcı ID'sini set ediyoruz
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
            var passwordVaults = await _passwordVaultRepository.GetListAsync(include: x => x.Include(p => p.User).Include(p => p.Category), cancellationToken: cancellationToken);
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
            var passwordVaults = await _passwordVaultRepository.GetListAsync(x => x.UserId == userId, include: x => x.Include(p => p.User).Include(p => p.Category), cancellationToken: cancellationToken);
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

    public async Task<IDataResult<PasswordVaultDto>> GetByIdAndUserAsync(Guid passwordVaultId, Guid userId, CancellationToken cancellationToken)
    {
        try
        {
            var passwordVault = await _passwordVaultRepository.GetAsync(
                x => x.Id == passwordVaultId && x.UserId == userId,
                include: x => x.Include(p => p.User).Include(p => p.Category),
                cancellationToken: cancellationToken);
            if (passwordVault == null)
                return new DataResult<PasswordVaultDto>(ResultStatus.Warning, "Password vault not found.", null);
            try
            {
                DecryptFields(passwordVault);
                var dto = _mapper.Map<PasswordVaultDto>(passwordVault);
                return new DataResult<PasswordVaultDto>(ResultStatus.Success, "Password vault retrieved successfully.", dto);
            }
            catch (CryptographicException ex)
            {
                _logger.LogError(ex, "Failed to decrypt password vault data for ID: {Id}", passwordVaultId);
                return new DataResult<PasswordVaultDto>(ResultStatus.Error, "Could not decrypt password vault data. The encryption key may have changed.", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing password vault with ID: {Id}", passwordVaultId);
                return new DataResult<PasswordVaultDto>(ResultStatus.Error, "An error occurred while processing the password vault.", null);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in {MethodName}. Failed to get password vault by Id. Details: {ExceptionMessage}",
                nameof(GetByIdAndUserAsync), ex.Message);
            return new DataResult<PasswordVaultDto>(ResultStatus.Error, "An error occurred while fetching the password vault.", null);
        }
    }

    public async Task<PasswordVaultDto> GetByIdAsync(Guid passwordVaultId, CancellationToken cancellationToken)
    {
        var passwordVault = await _passwordVaultRepository.GetAsync(x => x.Id == passwordVaultId, include: x => x.Include(p => p.User).Include(p => p.Category), cancellationToken: cancellationToken);
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
        passwordVault.PasswordHash = _encryption.Encrypt(dto.Password);
        passwordVault.SecureUsernameOrEmail = _encryption.Encrypt(dto.UsernameOrEmail);
    }

    private void DecryptFields(PasswordVault passwordVault)
    {
        passwordVault.SecureSiteName = _encryption.Decrypt(passwordVault.SecureSiteName);
        passwordVault.SecureNotes = _encryption.Decrypt(passwordVault.SecureNotes);
        passwordVault.PasswordHash = _encryption.Decrypt(passwordVault.PasswordHash);
        passwordVault.SecureUsernameOrEmail = _encryption.Decrypt(passwordVault.SecureUsernameOrEmail);
    }

    #region Password Health Monitoring

    public async Task<IDataResult<PasswordHealthReportDto>> GetPasswordHealthReportAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var vaults = await _passwordVaultRepository.GetListAsync(
                predicate: x => x.UserId == userId,
                include: x => x.Include(p => p.Category),
                cancellationToken: cancellationToken);

            // Decrypt all passwords for analysis (in memory only)
            var decryptedVaults = new List<PasswordVault>();
            foreach (var vault in vaults.Items)
            {
                try
                {
                    var decryptedVault = new PasswordVault
                    {
                        Id = vault.Id,
                        UserId = vault.UserId,
                        SecureSiteName = string.IsNullOrEmpty(vault.SecureSiteName) ? "" : _encryption.Decrypt(vault.SecureSiteName),
                        SecureUsernameOrEmail = string.IsNullOrEmpty(vault.SecureUsernameOrEmail) ? "" : _encryption.Decrypt(vault.SecureUsernameOrEmail),
                        PasswordHash = string.IsNullOrEmpty(vault.PasswordHash) ? "" : _encryption.Decrypt(vault.PasswordHash),
                        SecureNotes = string.IsNullOrEmpty(vault.SecureNotes) ? "" : _encryption.Decrypt(vault.SecureNotes),
                        LastPasswordChangeDate = vault.LastPasswordChangeDate,
                        CategoryId = vault.CategoryId
                    };
                    // BaseEntity properties from vault
                    decryptedVault.CreatedDate = vault.CreatedDate;
                    decryptedVault.UpdatedDate = vault.UpdatedDate;
                    decryptedVaults.Add(decryptedVault);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to decrypt vault {VaultId}, skipping from health analysis", vault.Id);
                    // Skip corrupted vaults from health analysis
                    continue;
                }
            }

            var report = AnalyzePasswordHealth(decryptedVaults);
            
            return new DataResult<PasswordHealthReportDto>(ResultStatus.Success, "Password health report generated successfully.", report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating password health report for user {UserId}", userId);
            return new DataResult<PasswordHealthReportDto>(ResultStatus.Error, "An error occurred while generating password health report.", null);
        }
    }

    public async Task<IDataResult<List<DuplicatePasswordDto>>> GetDuplicatePasswordsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var vaults = await _passwordVaultRepository.GetListAsync(
                predicate: x => x.UserId == userId,
                cancellationToken: cancellationToken);

            var duplicates = FindDuplicatePasswords(vaults.Items);
            
            return new DataResult<List<DuplicatePasswordDto>>(ResultStatus.Success, "Duplicate passwords analyzed successfully.", duplicates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing duplicate passwords for user {UserId}", userId);
            return new DataResult<List<DuplicatePasswordDto>>(ResultStatus.Error, "An error occurred while analyzing duplicate passwords.", null);
        }
    }

    public async Task<IDataResult<List<WeakPasswordDto>>> GetWeakPasswordsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var vaults = await _passwordVaultRepository.GetListAsync(
                predicate: x => x.UserId == userId,
                cancellationToken: cancellationToken);

            var weakPasswords = FindWeakPasswords(vaults.Items);
            
            return new DataResult<List<WeakPasswordDto>>(ResultStatus.Success, "Weak passwords analyzed successfully.", weakPasswords);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing weak passwords for user {UserId}", userId);
            return new DataResult<List<WeakPasswordDto>>(ResultStatus.Error, "An error occurred while analyzing weak passwords.", null);
        }
    }

    public async Task<IDataResult<SecurityScoreDto>> GetSecurityScoreAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        try
        {
            var vaults = await _passwordVaultRepository.GetListAsync(
                predicate: x => x.UserId == userId,
                cancellationToken: cancellationToken);

            var securityScore = CalculateSecurityScore(vaults.Items);
            
            return new DataResult<SecurityScoreDto>(ResultStatus.Success, "Security score calculated successfully.", securityScore);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating security score for user {UserId}", userId);
            return new DataResult<SecurityScoreDto>(ResultStatus.Error, "An error occurred while calculating security score.", null);
        }
    }

    private PasswordHealthReportDto AnalyzePasswordHealth(List<PasswordVault> decryptedVaults)
    {
        var totalPasswords = decryptedVaults.Count;
        var weakPasswords = CountWeakPasswordsFromDecrypted(decryptedVaults);
        var duplicatePasswords = CountDuplicatePasswordsFromDecrypted(decryptedVaults);
        var oldPasswords = CountOldPasswords(decryptedVaults);
        var shortPasswords = CountShortPasswordsFromDecrypted(decryptedVaults);
        var strongPasswords = totalPasswords - weakPasswords - shortPasswords;

        var securityScore = CalculateOverallSecurityScore(totalPasswords, weakPasswords, duplicatePasswords, oldPasswords, shortPasswords);
        var securityLevel = GetSecurityLevel(securityScore);

        var issues = GenerateSecurityIssues(weakPasswords, duplicatePasswords, oldPasswords, shortPasswords);
        var recommendations = GenerateRecommendations(issues);

        return new PasswordHealthReportDto
        {
            SecurityScore = securityScore,
            SecurityLevel = securityLevel,
            TotalPasswords = totalPasswords,
            WeakPasswords = weakPasswords,
            DuplicatePasswords = duplicatePasswords,
            OldPasswords = oldPasswords,
            ShortPasswords = shortPasswords,
            StrongPasswords = strongPasswords,
            Issues = issues,
            Recommendations = recommendations
        };
    }

    private List<DuplicatePasswordDto> FindDuplicatePasswords(ICollection<PasswordVault> vaults)
    {
        var duplicates = new List<DuplicatePasswordDto>();
        var passwordGroups = new Dictionary<string, List<PasswordVault>>();

        // Group by decrypted password
        foreach (var vault in vaults)
        {
            var decryptedPassword = _encryption.Decrypt(vault.PasswordHash);
            
            if (!passwordGroups.ContainsKey(decryptedPassword))
                passwordGroups[decryptedPassword] = new List<PasswordVault>();
            
            passwordGroups[decryptedPassword].Add(vault);
        }

        // Find duplicates (groups with more than 1 vault)
        foreach (var group in passwordGroups.Where(g => g.Value.Count > 1))
        {
            var mainSite = group.Value.First();
            var duplicateSites = group.Value.Skip(1).Select(v => _encryption.Decrypt(v.SecureSiteName)).ToList();

            duplicates.Add(new DuplicatePasswordDto
            {
                SiteName = _encryption.Decrypt(mainSite.SecureSiteName),
                DuplicateSites = duplicateSites,
                UsageCount = group.Value.Count,
                LastUsed = group.Value.Max(v => v.UpdatedDate ?? v.CreatedDate)
            });
        }

        return duplicates;
    }

    private List<WeakPasswordDto> FindWeakPasswords(ICollection<PasswordVault> vaults)
    {
        var weakPasswords = new List<WeakPasswordDto>();
        var commonWeakPasswords = GetCommonWeakPasswords();

        foreach (var vault in vaults)
        {
            var decryptedPassword = _encryption.Decrypt(vault.PasswordHash);
            var weaknessReason = GetPasswordWeaknessReason(decryptedPassword, commonWeakPasswords);

            if (!string.IsNullOrEmpty(weaknessReason))
            {
                weakPasswords.Add(new WeakPasswordDto
                {
                    VaultId = vault.Id,
                    SiteName = _encryption.Decrypt(vault.SecureSiteName),
                    Username = _encryption.Decrypt(vault.SecureUsernameOrEmail),
                    WeaknessReason = weaknessReason,
                    PasswordLength = decryptedPassword.Length,
                    CreatedAt = vault.CreatedDate
                });
            }
        }

        return weakPasswords;
    }

    private SecurityScoreDto CalculateSecurityScore(ICollection<PasswordVault> vaults)
    {
        var totalPasswords = vaults.Count;
        if (totalPasswords == 0)
        {
            return new SecurityScoreDto
            {
                OverallScore = 0,
                Level = "No Passwords",
                LevelColor = "gray",
                LevelIcon = "🔒",
                LastAnalyzed = DateTime.UtcNow
            };
        }

        var weakPasswords = CountWeakPasswords(vaults.ToList());
        var duplicatePasswords = CountDuplicatePasswords(vaults.ToList());
        var oldPasswords = CountOldPasswords(vaults.ToList());
        var shortPasswords = CountShortPasswords(vaults.ToList());

        var overallScore = CalculateOverallSecurityScore(totalPasswords, weakPasswords, duplicatePasswords, oldPasswords, shortPasswords);
        var level = GetSecurityLevel(overallScore);
        var (color, icon) = GetSecurityLevelDisplay(level);

        var categoryScores = new Dictionary<string, int>
        {
            ["Strength"] = Math.Max(0, 100 - (weakPasswords * 100 / totalPasswords)),
            ["Uniqueness"] = Math.Max(0, 100 - (duplicatePasswords * 100 / totalPasswords)),
            ["Freshness"] = Math.Max(0, 100 - (oldPasswords * 100 / totalPasswords)),
            ["Length"] = Math.Max(0, 100 - (shortPasswords * 100 / totalPasswords))
        };

        var improvementPotential = 100 - overallScore;

        return new SecurityScoreDto
        {
            OverallScore = overallScore,
            Level = level,
            LevelColor = color,
            LevelIcon = icon,
            CategoryScores = categoryScores,
            ImprovementPotential = improvementPotential,
            LastAnalyzed = DateTime.UtcNow
        };
    }

    #region Password Analysis Helper Methods

    private int CountWeakPasswordsFromDecrypted(List<PasswordVault> decryptedVaults)
    {
        var commonWeakPasswords = GetCommonWeakPasswords();
        var count = 0;

        foreach (var vault in decryptedVaults)
        {
            // vault.PasswordHash is already decrypted
            if (!string.IsNullOrEmpty(GetPasswordWeaknessReason(vault.PasswordHash, commonWeakPasswords)))
                count++;
        }

        return count;
    }

    private int CountDuplicatePasswordsFromDecrypted(List<PasswordVault> decryptedVaults)
    {
        var passwordCounts = new Dictionary<string, int>();

        foreach (var vault in decryptedVaults)
        {
            // vault.PasswordHash is already decrypted
            passwordCounts[vault.PasswordHash] = passwordCounts.GetValueOrDefault(vault.PasswordHash, 0) + 1;
        }

        return passwordCounts.Values.Where(count => count > 1).Sum() - passwordCounts.Count(kvp => kvp.Value > 1);
    }

    private int CountShortPasswordsFromDecrypted(List<PasswordVault> decryptedVaults)
    {
        var count = 0;
        foreach (var vault in decryptedVaults)
        {
            // vault.PasswordHash is already decrypted
            if (vault.PasswordHash.Length < 8)
                count++;
        }
        return count;
    }

    private int CountWeakPasswords(List<PasswordVault> vaults)
    {
        var commonWeakPasswords = GetCommonWeakPasswords();
        var count = 0;

        foreach (var vault in vaults)
        {
            var decryptedPassword = _encryption.Decrypt(vault.PasswordHash);
            if (!string.IsNullOrEmpty(GetPasswordWeaknessReason(decryptedPassword, commonWeakPasswords)))
                count++;
        }

        return count;
    }

    private int CountDuplicatePasswords(List<PasswordVault> vaults)
    {
        var passwordCounts = new Dictionary<string, int>();

        foreach (var vault in vaults)
        {
            var decryptedPassword = _encryption.Decrypt(vault.PasswordHash);
            passwordCounts[decryptedPassword] = passwordCounts.GetValueOrDefault(decryptedPassword, 0) + 1;
        }

        return passwordCounts.Values.Where(count => count > 1).Sum() - passwordCounts.Count(kvp => kvp.Value > 1);
    }

    private int CountOldPasswords(List<PasswordVault> vaults)
    {
        var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
        return vaults.Count(v => (v.UpdatedDate ?? v.CreatedDate) < sixMonthsAgo);
    }

    private int CountShortPasswords(List<PasswordVault> vaults)
    {
        var count = 0;
        foreach (var vault in vaults)
        {
            var decryptedPassword = _encryption.Decrypt(vault.PasswordHash);
            if (decryptedPassword.Length < 8)
                count++;
        }
        return count;
    }

    private int CalculateOverallSecurityScore(int total, int weak, int duplicate, int old, int shortPw)
    {
        if (total == 0) return 0;

        var baseScore = 100;
        
        // Deduct points for security issues
        baseScore -= (weak * 25 / Math.Max(total, 1));        // Weak passwords: -25 points per password
        baseScore -= (duplicate * 15 / Math.Max(total, 1));   // Duplicate passwords: -15 points per password
        baseScore -= (old * 10 / Math.Max(total, 1));         // Old passwords: -10 points per password
        baseScore -= (shortPw * 10 / Math.Max(total, 1));     // Short passwords: -10 points per password

        return Math.Max(0, Math.Min(100, baseScore));
    }

    private string GetSecurityLevel(int score)
    {
        return score switch
        {
            >= 90 => "Excellent",
            >= 70 => "Good", 
            >= 50 => "Fair",
            _ => "Poor"
        };
    }

    private (string color, string icon) GetSecurityLevelDisplay(string level)
    {
        return level switch
        {
            "Excellent" => ("green", "🛡️"),
            "Good" => ("blue", "🔒"),
            "Fair" => ("orange", "⚠️"),
            "Poor" => ("red", "🚨"),
            _ => ("gray", "❓")
        };
    }

    private List<SecurityIssueDto> GenerateSecurityIssues(int weak, int duplicate, int old, int shortPw)
    {
        var issues = new List<SecurityIssueDto>();

        if (weak > 0)
            issues.Add(new SecurityIssueDto
            {
                Type = "Weak",
                Message = $"{weak} weak password{(weak > 1 ? "s" : "")} detected",
                Count = weak,
                Severity = "High",
                Icon = "🔓"
            });

        if (duplicate > 0)
            issues.Add(new SecurityIssueDto
            {
                Type = "Duplicate", 
                Message = $"{duplicate} password{(duplicate > 1 ? "s" : "")} used multiple times",
                Count = duplicate,
                Severity = "Medium",
                Icon = "🔄"
            });

        if (old > 0)
            issues.Add(new SecurityIssueDto
            {
                Type = "Old",
                Message = $"{old} password{(old > 1 ? "s" : "")} not changed in 6+ months",
                Count = old,
                Severity = "Medium",
                Icon = "⏰"
            });

        if (shortPw > 0)
            issues.Add(new SecurityIssueDto
            {
                Type = "Short",
                Message = $"{shortPw} password{(shortPw > 1 ? "s" : "")} shorter than 8 characters",
                Count = shortPw,
                Severity = "Medium",
                Icon = "📏"
            });

        return issues;
    }

    private List<SecurityRecommendationDto> GenerateRecommendations(List<SecurityIssueDto> issues)
    {
        var recommendations = new List<SecurityRecommendationDto>();

        if (issues.Any(i => i.Type == "Weak"))
            recommendations.Add(new SecurityRecommendationDto
            {
                Title = "Strengthen Weak Passwords",
                Description = "Replace weak passwords with strong, randomly generated ones",
                Action = "Use password generator for better security",
                Priority = "High"
            });

        if (issues.Any(i => i.Type == "Duplicate"))
            recommendations.Add(new SecurityRecommendationDto
            {
                Title = "Use Unique Passwords",
                Description = "Each account should have its own unique password",
                Action = "Generate unique passwords for duplicate accounts",
                Priority = "High"
            });

        if (issues.Any(i => i.Type == "Old"))
            recommendations.Add(new SecurityRecommendationDto
            {
                Title = "Update Old Passwords",
                Description = "Change passwords that haven't been updated recently",
                Action = "Update passwords older than 6 months",
                Priority = "Medium"
            });

        if (issues.Any(i => i.Type == "Short"))
            recommendations.Add(new SecurityRecommendationDto
            {
                Title = "Use Longer Passwords",
                Description = "Passwords should be at least 8 characters long",
                Action = "Extend short passwords to 12+ characters",
                Priority = "Medium"
            });

        return recommendations;
    }

    private HashSet<string> GetCommonWeakPasswords()
    {
        return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "123456", "password", "123456789", "12345678", "12345", "1234567",
            "admin", "welcome", "password123", "123123", "qwerty", "abc123",
            "letmein", "master", "login", "pass", "secret", "admin123",
            "password1", "123321", "654321", "111111", "000000", "iloveyou"
        };
    }

    private string GetPasswordWeaknessReason(string password, HashSet<string> commonWeakPasswords)
    {
        if (string.IsNullOrWhiteSpace(password))
            return "Empty password";

        if (password.Length < 4)
            return "Too short (less than 4 characters)";

        if (commonWeakPasswords.Contains(password))
            return "Common weak password";

        if (password.All(char.IsDigit))
            return "Only numbers";

        if (password.All(char.IsLower))
            return "Only lowercase letters";

        if (password.Length < 6 && !password.Any(char.IsDigit) && !password.Any(char.IsSymbol))
            return "Too simple and short";

        return string.Empty; // Not weak
    }

    #endregion

    #endregion
}