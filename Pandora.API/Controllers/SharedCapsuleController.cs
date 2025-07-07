using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pandora.Application.Interfaces.Repositories;
using Pandora.Application.Interfaces.Security;
using Swashbuckle.AspNetCore.Annotations;

namespace Pandora.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SharedCapsuleController : ControllerBase
{
    private readonly IPersonalVaultRepository _personalVaultRepository;
    private readonly IEncryption _encryption;

    public SharedCapsuleController(IPersonalVaultRepository personalVaultRepository, IEncryption encryption)
    {
        _personalVaultRepository = personalVaultRepository;
        _encryption = encryption;
    }

    /// <summary>
    /// Retrieves a shared time capsule by its share token (anonymous access).
    /// </summary>
    [HttpGet("{shareToken}")]
    [SwaggerOperation(Summary = "View shared time capsule", Description = "Access a shared time capsule using its share token. No authentication required.")]
    [SwaggerResponse(200, "Time capsule found")]
    [SwaggerResponse(404, "Time capsule not found")]
    [SwaggerResponse(423, "Time capsule is still locked")]
    public async Task<IActionResult> GetSharedCapsuleAsync(string shareToken, CancellationToken cancellationToken)
    {
        var vault = await _personalVaultRepository.GetAsync(
            x => x.ShareToken == shareToken && x.IsShareable,
            include: x => x.Include(p => p.User),
            cancellationToken: cancellationToken);

        if (vault == null)
        {
            return NotFound(new { Message = "🔍 Paylaşılan zaman kapsülü bulunamadı." });
        }

        // View count'u artır
        vault.ShareViewCount++;
        await _personalVaultRepository.UpdateAsync(vault, cancellationToken);

        var isLocked = vault.IsLocked && vault.UnlockDate.HasValue && vault.UnlockDate.Value > DateTime.UtcNow;

        if (isLocked)
        {
            // Kilitli - sadece temel bilgileri göster
            var lockedResponse = new
            {
                Title = _encryption.Decrypt(vault.SecureTitle),
                IsLocked = true,
                UnlockDate = vault.UnlockDate,
                RemainingTime = vault.UnlockDate - DateTime.UtcNow,
                SharedBy = vault.User?.Username,
                ViewCount = vault.ShareViewCount,
                Message = "🔒 Bu zaman kapsülü henüz kilitli. Açılış tarihini bekleyin!"
            };
            return StatusCode(423, lockedResponse); // 423 Locked
        }
        else
        {
            // Kilit açık - tüm içeriği göster
            var unlockedResponse = new
            {
                Title = _encryption.Decrypt(vault.SecureTitle),
                Content = _encryption.Decrypt(vault.SecureContent),
                Summary = _encryption.Decrypt(vault.SecureSummary),
                Url = _encryption.Decrypt(vault.SecureUrl),
                MediaFile = _encryption.Decrypt(vault.SecureMediaFile),
                Tags = vault.SecureTags?.Select(tag => _encryption.Decrypt(tag)).ToList(),
                IsLocked = false,
                UnlockedAt = vault.UnlockDate,
                SharedBy = vault.User?.Username,
                ViewCount = vault.ShareViewCount,
                CreatedDate = vault.CreatedDate,
                Message = "🎉 Zaman kapsülü açıldı! İçeriği keşfedin."
            };
            return Ok(unlockedResponse);
        }
    }

    /// <summary>
    /// Gets the status of a shared time capsule (locked/unlocked info).
    /// </summary>
    [HttpGet("{shareToken}/status")]
    [SwaggerOperation(Summary = "Get time capsule status", Description = "Check if a time capsule is locked or unlocked without incrementing view count.")]
    [SwaggerResponse(200, "Status retrieved")]
    [SwaggerResponse(404, "Time capsule not found")]
    public async Task<IActionResult> GetCapsuleStatusAsync(string shareToken, CancellationToken cancellationToken)
    {
        var vault = await _personalVaultRepository.GetAsync(
            x => x.ShareToken == shareToken && x.IsShareable,
            cancellationToken: cancellationToken);

        if (vault == null)
        {
            return NotFound(new { Message = "Zaman kapsülü bulunamadı." });
        }

        var isLocked = vault.IsLocked && vault.UnlockDate.HasValue && vault.UnlockDate.Value > DateTime.UtcNow;

        var statusResponse = new
        {
            IsLocked = isLocked,
            UnlockDate = vault.UnlockDate,
            RemainingTime = isLocked ? vault.UnlockDate - DateTime.UtcNow : TimeSpan.Zero,
            ViewCount = vault.ShareViewCount,
            Title = _encryption.Decrypt(vault.SecureTitle) // Başlık her zaman gösterilir
        };

        return Ok(statusResponse);
    }
} 