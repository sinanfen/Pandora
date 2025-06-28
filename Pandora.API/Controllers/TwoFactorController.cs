using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Results;
using Pandora.Shared.DTOs.AuthDTOs;
using System.Security.Claims;

namespace Pandora.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // All endpoints require authentication
public class TwoFactorController : ControllerBase
{
    private readonly IAuthService _authService;

    public TwoFactorController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// 🔐 Get user's 2FA status
    /// </summary>
    [HttpGet("status")]
    public async Task<IActionResult> GetTwoFactorStatus(CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();
        var result = await _authService.GetTwoFactorStatusAsync(userId, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    /// <summary>
    /// 🔧 Setup 2FA - Generate QR code and backup codes
    /// </summary>
    [HttpPost("setup")]
    public async Task<IActionResult> SetupTwoFactor(CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();
        var result = await _authService.SetupTwoFactorAsync(userId, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    /// <summary>
    /// ✅ Enable 2FA after verification
    /// </summary>
    [HttpPost("enable")]
    public async Task<IActionResult> EnableTwoFactor([FromBody] TwoFactorToggleDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetLoggedInUserId();
        var result = await _authService.EnableTwoFactorAsync(userId, dto, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    /// <summary>
    /// ❌ Disable 2FA
    /// </summary>
    [HttpPost("disable")]
    public async Task<IActionResult> DisableTwoFactor([FromBody] TwoFactorToggleDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetLoggedInUserId();
        var result = await _authService.DisableTwoFactorAsync(userId, dto, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    /// <summary>
    /// 🔄 Generate new backup codes
    /// </summary>
    [HttpPost("backup-codes/regenerate")]
    public async Task<IActionResult> RegenerateBackupCodes([FromBody] GenerateBackupCodesDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetLoggedInUserId();
        var result = await _authService.GenerateNewBackupCodesAsync(userId, dto.CurrentPassword, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    /// <summary>
    /// 🔄 Resend 2FA setup (same secret if exists, new if forced)
    /// </summary>
    [HttpPost("setup/resend")]
    public async Task<IActionResult> ResendTwoFactorSetup([FromBody] ResendSetupDto dto, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();
        var result = await _authService.ResendTwoFactorSetupAsync(userId, dto.ForceNew, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    private Guid GetLoggedInUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdClaim, out var userId))
            return userId;
        
        throw new UnauthorizedAccessException("User ID not found in token");
    }
}

public class GenerateBackupCodesDto
{
    public string CurrentPassword { get; set; } = string.Empty;
}

public class ResendSetupDto
{
    public bool ForceNew { get; set; } = false;
} 