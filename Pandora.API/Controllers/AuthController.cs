using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Results;
using Pandora.Shared.DTOs.UserDTOs;

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

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDto dto, CancellationToken cancellationToken)
    {
        var result = await _authService.LoginAsync(dto, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return Ok(new { Success = false, Message = result.Message });
        return Ok(new { Success = true, Token = result.Data, Message = result.Message });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken)
    {
        var result = await _userService.RegisterUserAsync(userRegisterDto, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return Ok(new { Success = false, Message = result.Message });
        return Ok(new { Success = true, Data = result.Data, Message = result.Message });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] UserPasswordChangeDto userPasswordChangeDto, CancellationToken cancellationToken)
    {
        var result = await _userService.ChangePasswordAsync(userPasswordChangeDto, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(result); // Return structured JSON
        return Ok(result); // Success message in structured JSON
    }

    // Token doğrulama
    [HttpPost("validate-token")]
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