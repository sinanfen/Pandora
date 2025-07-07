using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Results;
using Pandora.Shared.DTOs.AuthDTOs;
using Pandora.Shared.DTOs.UserDTOs;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Pandora.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    /// <summary>
    /// Get client IP address
    /// </summary>
    private string GetClientIpAddress()
    {
        return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    /// <summary>
    /// Get client user agent
    /// </summary>
    private string GetClientUserAgent()
    {
        return HttpContext.Request.Headers["User-Agent"].FirstOrDefault() ?? "Unknown";
    }

    /// <summary>
    /// Get current user ID from JWT token
    /// </summary>
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID not found in token");
        return Guid.Parse(userIdClaim.Value);
    }

    /// <summary>
    /// User login with username/email and password
    /// </summary>
    /// <param name="dto">Login credentials</param>
    /// <returns>Access token and refresh token</returns>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "User login", Description = "Authenticate user and return access token with refresh token")]
    [SwaggerResponse(200, "Login successful", typeof(TokenDto))]
    [SwaggerResponse(400, "Invalid credentials")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var ipAddress = GetClientIpAddress();
        var userAgent = GetClientUserAgent();

        var result = await _authService.LoginAsync(dto, ipAddress, userAgent, cancellationToken);
        
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });

        return Ok(result);
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    /// <param name="dto">Refresh token</param>
    /// <returns>New access token and refresh token</returns>
    [HttpPost("refresh")]
    [SwaggerOperation(Summary = "Refresh token", Description = "Generate new access token using refresh token")]
    [SwaggerResponse(200, "Token refreshed successfully", typeof(TokenDto))]
    [SwaggerResponse(400, "Invalid refresh token")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var ipAddress = GetClientIpAddress();
        var userAgent = GetClientUserAgent();

        var result = await _authService.RefreshTokenAsync(dto, ipAddress, userAgent, cancellationToken);
        
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });

        return Ok(result);
    }

    /// <summary>
    /// Revoke a refresh token
    /// </summary>
    /// <param name="dto">Token to revoke</param>
    /// <returns>Success message</returns>
    [HttpPost("revoke")]
    [SwaggerOperation(Summary = "Revoke token", Description = "Revoke a specific refresh token")]
    [SwaggerResponse(200, "Token revoked successfully")]
    [SwaggerResponse(400, "Invalid token")]
    public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _authService.RevokeTokenAsync(dto, cancellationToken);
        
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });

        return Ok(result);
    }

    /// <summary>
    /// Logout user and revoke all tokens
    /// </summary>
    /// <returns>Success message</returns>
    [HttpPost("logout")]
    [Authorize]
    [SwaggerOperation(Summary = "Logout", Description = "Logout user and revoke all refresh tokens")]
    [SwaggerResponse(200, "Logged out successfully")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> LogoutAsync(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var result = await _authService.LogoutAsync(userId, cancellationToken);
        
        return Ok(result);
    }

    /// <summary>
    /// Get user's active sessions
    /// </summary>
    /// <param name="currentRefreshToken">Current refresh token to identify current session</param>
    /// <returns>List of active sessions</returns>
    [HttpGet("sessions")]
    [Authorize]
    [SwaggerOperation(Summary = "Get active sessions", Description = "Get all active sessions for the current user")]
    [SwaggerResponse(200, "Active sessions retrieved", typeof(List<SessionDto>))]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> GetActiveSessionsAsync([FromQuery] string currentRefreshToken, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var result = await _authService.GetActiveSessionsAsync(userId, currentRefreshToken, cancellationToken);
        
        return Ok(result);
    }

    /// <summary>
    /// Revoke all other sessions except current
    /// </summary>
    /// <param name="currentRefreshToken">Current refresh token to keep active</param>
    /// <returns>Success message</returns>
    [HttpPost("logout-others")]
    [Authorize]
    [SwaggerOperation(Summary = "Logout other sessions", Description = "Revoke all refresh tokens except the current one")]
    [SwaggerResponse(200, "Other sessions logged out")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> LogoutOtherSessionsAsync([FromBody] string currentRefreshToken, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var result = await _authService.RevokeAllOtherSessionsAsync(userId, currentRefreshToken, cancellationToken);
        
        return Ok(result);
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="dto">User registration data</param>
    /// <returns>Registration result</returns>
    [HttpPost("register")]
    [SwaggerOperation(Summary = "Register user", Description = "Register a new user account")]
    [SwaggerResponse(200, "User registered successfully")]
    [SwaggerResponse(400, "Registration failed")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _userService.RegisterUserAsync(dto, cancellationToken);
        
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });

        // Send email verification after successful registration
        try
        {
            var ipAddress = GetClientIpAddress();
            var userAgent = GetClientUserAgent();
            
            await _authService.SendEmailVerificationAsync(dto.Email, ipAddress, userAgent, cancellationToken);
            
            return Ok(new { 
                Result = result.ResultStatus, 
                Message = "User successfully registered. Please check your email for verification instructions.",
                Data = result.Data 
            });
        }
        catch (Exception)
        {
            return Ok(new { 
                Result = result.ResultStatus, 
                Message = "User successfully registered, but verification email could not be sent. Please request a new verification email.",
                Data = result.Data 
            });
        }
    }

    /// <summary>
    /// Changes the password. Requires user to be logged in.
    /// </summary>
    [Authorize]
    [HttpPost("change-password")]
    [SwaggerOperation(Summary = "Change password", Description = "Changes the password for the logged-in user.")]
    [SwaggerResponse(200, "Password changed successfully")]
    [SwaggerResponse(400, "Invalid request")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] UserPasswordChangeDto userPasswordChangeDto, CancellationToken cancellationToken)
    {
        var result = await _userService.ChangePasswordAsync(userPasswordChangeDto, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(result); // Return structured JSON
        return Ok(result); // Success message in structured JSON
    }

    /// <summary>
    /// Validates whether the JWT token is still active/valid.
    /// </summary>
    [HttpPost("validate-token")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Token validation", Description = "Checks if the JWT token is valid.")]
    [SwaggerResponse(200, "Token is valid")]
    [SwaggerResponse(401, "Invalid token")]
    public IActionResult ValidateToken(string token)
    {
        try
        {
            var principal = _authService.GetPrincipalFromExpiredToken(token);
            return Ok(new { Message = "Token is valid", User = principal.Identity.Name });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { Message = "Invalid token", Error = ex.Message });
        }
    }

    /// <summary>
    /// Verify email address with token
    /// </summary>
    [HttpPost("verify-email")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Verify email address", Description = "Verifies user's email address using the verification token.")]
    [SwaggerResponse(200, "Email verified successfully")]
    [SwaggerResponse(400, "Invalid or expired token")]
    public async Task<IActionResult> VerifyEmailAsync([FromBody] EmailVerificationDto emailVerificationDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _authService.VerifyEmailAsync(emailVerificationDto, cancellationToken);
        
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });

        return Ok(result);
    }

    /// <summary>
    /// Resend email verification
    /// </summary>
    [HttpPost("resend-verification")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Resend verification email", Description = "Resends verification email to the specified address.")]
    [SwaggerResponse(200, "Verification email sent")]
    [SwaggerResponse(400, "Invalid email or already verified")]
    public async Task<IActionResult> ResendEmailVerificationAsync([FromBody] ResendEmailVerificationDto resendEmailDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var ipAddress = GetClientIpAddress();
        var userAgent = GetClientUserAgent();

        var result = await _authService.ResendEmailVerificationAsync(resendEmailDto, ipAddress, userAgent, cancellationToken);
        
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });

        return Ok(result);
    }

    /// <summary>
    /// Check email verification status
    /// </summary>
    [HttpGet("email-status")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Check email verification status", Description = "Checks if an email address is verified.")]
    [SwaggerResponse(200, "Email status retrieved")]
    [SwaggerResponse(404, "User not found")]
    public async Task<IActionResult> CheckEmailStatusAsync([FromQuery] string email, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(email))
            return BadRequest("Email parameter is required");

        var result = await _authService.IsEmailVerifiedAsync(email, cancellationToken);
        
        if (result.ResultStatus != ResultStatus.Success)
            return NotFound(new { Result = result.ResultStatus, Message = result.Message });

        return Ok(new { Email = email, IsVerified = result.Data });
    }

    /// <summary>
    /// Change user's email address
    /// </summary>
    [HttpPost("change-email")]
    [Authorize]
    [SwaggerOperation(Summary = "Change email address", Description = "Changes user's email address. Requires current password for security.")]
    [SwaggerResponse(200, "Email changed successfully")]
    [SwaggerResponse(400, "Invalid input or password")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailDto changeEmailDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var userId = GetCurrentUserId();
        var ipAddress = GetClientIpAddress();
        var userAgent = GetClientUserAgent();

        var result = await _authService.ChangeEmailAsync(userId, changeEmailDto, ipAddress, userAgent, cancellationToken);
        
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });

        return Ok(result);
    }

    /// <summary>
    /// 🔐 Complete two-factor authentication login
    /// </summary>
    [HttpPost("verify-2fa")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Verify 2FA code", Description = "Complete the two-factor authentication process after initial login")]
    [SwaggerResponse(200, "2FA verification successful", typeof(TokenDto))]
    [SwaggerResponse(400, "Invalid 2FA code or temp token")]
    public async Task<IActionResult> VerifyTwoFactorAsync([FromBody] TwoFactorLoginDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var ipAddress = GetClientIpAddress();
        var userAgent = GetClientUserAgent();
        
        var result = await _authService.VerifyTwoFactorAsync(dto, ipAddress, userAgent, cancellationToken);
        
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });

        return Ok(result);
    }

    /// <summary>
    /// 🔄 Resend temp token for 2FA login
    /// </summary>
    [HttpPost("resend-temp-token")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Resend temp token", Description = "Generate new temporary token for 2FA login when the previous one expired")]
    [SwaggerResponse(200, "New temp token generated", typeof(TokenDto))]
    [SwaggerResponse(400, "Invalid credentials or 2FA not enabled")]
    public async Task<IActionResult> ResendTempTokenAsync([FromBody] UserLoginDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var ipAddress = GetClientIpAddress();
        var userAgent = GetClientUserAgent();
        
        var result = await _authService.ResendTempTokenAsync(dto, ipAddress, userAgent, cancellationToken);
        
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });

        return Ok(result);
    }

    #region Two-Factor Authentication Endpoints

    /// <summary>
    /// 🔐 Get user's 2FA status
    /// </summary>
    [HttpGet("2fa/status")]
    [Authorize]
    [SwaggerOperation(Summary = "Get 2FA status", Description = "Get the current two-factor authentication status for the logged-in user")]
    [SwaggerResponse(200, "2FA status retrieved")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> GetTwoFactorStatus(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var result = await _authService.GetTwoFactorStatusAsync(userId, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    /// <summary>
    /// 🔧 Setup 2FA - Generate QR code and backup codes
    /// </summary>
    [HttpPost("2fa/setup")]
    [Authorize]
    [SwaggerOperation(Summary = "Setup 2FA", Description = "Generate QR code and backup codes for two-factor authentication setup")]
    [SwaggerResponse(200, "2FA setup generated")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> SetupTwoFactor(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var result = await _authService.SetupTwoFactorAsync(userId, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    /// <summary>
    /// ✅ Enable 2FA after verification
    /// </summary>
    [HttpPost("2fa/enable")]
    [Authorize]
    [SwaggerOperation(Summary = "Enable 2FA", Description = "Enable two-factor authentication after verification")]
    [SwaggerResponse(200, "2FA enabled successfully")]
    [SwaggerResponse(400, "Invalid verification code or password")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> EnableTwoFactor([FromBody] TwoFactorToggleDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        var result = await _authService.EnableTwoFactorAsync(userId, dto, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    /// <summary>
    /// ❌ Disable 2FA
    /// </summary>
    [HttpPost("2fa/disable")]
    [Authorize]
    [SwaggerOperation(Summary = "Disable 2FA", Description = "Disable two-factor authentication")]
    [SwaggerResponse(200, "2FA disabled successfully")]
    [SwaggerResponse(400, "Invalid password")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> DisableTwoFactor([FromBody] TwoFactorToggleDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        var result = await _authService.DisableTwoFactorAsync(userId, dto, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    /// <summary>
    /// 🔄 Generate new backup codes
    /// </summary>
    [HttpPost("2fa/backup-codes/regenerate")]
    [Authorize]
    [SwaggerOperation(Summary = "Regenerate backup codes", Description = "Generate new backup codes for two-factor authentication")]
    [SwaggerResponse(200, "New backup codes generated")]
    [SwaggerResponse(400, "Invalid current password")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> RegenerateBackupCodes([FromBody] GenerateBackupCodesDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        var result = await _authService.GenerateNewBackupCodesAsync(userId, dto.CurrentPassword, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    /// <summary>
    /// 🔄 Resend 2FA setup (same secret if exists, new if forced)
    /// </summary>
    [HttpPost("2fa/setup/resend")]
    [Authorize]
    [SwaggerOperation(Summary = "Resend 2FA setup", Description = "Resend two-factor authentication setup with same or new secret")]
    [SwaggerResponse(200, "2FA setup resent")]
    [SwaggerResponse(401, "Unauthorized")]
    public async Task<IActionResult> ResendTwoFactorSetup([FromBody] ResendSetupDto dto, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();
        var result = await _authService.ResendTwoFactorSetupAsync(userId, dto.ForceNew, cancellationToken);
        return StatusCode((int)result.ResultStatus, result);
    }

    #endregion
}

public class GenerateBackupCodesDto
{
    public string CurrentPassword { get; set; } = string.Empty;
}

public class ResendSetupDto
{
    public bool ForceNew { get; set; } = false;
}