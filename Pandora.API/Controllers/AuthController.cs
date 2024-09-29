using Microsoft.AspNetCore.Mvc;
using Pandora.Application.DTOs.UserDTOs;
using Pandora.Application.Interfaces;
using Pandora.Application.Utilities.Results;

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
    public async Task<IActionResult> Login(UserLoginDto dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result.ResultStatus == ResultStatus.Success)
        {
            return Ok(new { Token = result.Data, Message = result.Message });
        }
        return Unauthorized(result.Message);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(UserRegisterDto userRegisterDto)
    {
        var cts = new CancellationTokenSource();
        var result = await _userService.RegisterUserAsync(userRegisterDto, cts.Token);
        return Ok(result);
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