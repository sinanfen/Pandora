using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Results;
using Pandora.Shared.DTOs.UserDTOs;
using Swashbuckle.AspNetCore.Annotations;

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
    /// Performs user login and returns a JWT token.
    /// </summary>
    /// <param name="dto">User login credentials</param>
    /// <returns>Response containing JWT token</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [EnableRateLimiting("LoginPolicy")]
    [SwaggerOperation(Summary = "Performs user login", Description = "Logs in with email and password. Returns a JWT token.")]
    [SwaggerResponse(200, "Login successful")]
    [SwaggerResponse(400, "Invalid login")]
    public async Task<IActionResult> Login(UserLoginDto dto, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(dto, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Success = false, Message = result.Message });
        return Ok(new { Success = true, Token = result.Data, Message = result.Message });
    }

    /// <summary>
    /// Creates a new user registration.
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "New user registration", Description = "Registers a new user in the system.")]
    [SwaggerResponse(200, "Registration successful")]
    [SwaggerResponse(400, "Invalid information")]
    public async Task<IActionResult> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken)
    {
        IDataResult<UserDto> result = await _userService.RegisterUserAsync(userRegisterDto, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Success = false, Message = result.Message });
        return Ok(new { Success = true, Data = result.Data, Message = result.Message });
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
            return Ok(new { Message = "Token geçerli", User = principal.Identity.Name });
        }
        catch (Exception ex)
        {
            return Unauthorized(new { Message = "Token geçersiz", Error = ex.Message });
        }
    }
}