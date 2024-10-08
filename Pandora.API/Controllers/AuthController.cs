﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.Utilities.Results;
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
        if (result.ResultStatus == ResultStatus.Success)
        {
            return Ok(new { Token = result.Data, Message = result.Message });
        }
        return Unauthorized(result.Message);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(UserRegisterDto userRegisterDto, CancellationToken cancellationToken)
    {
        var result = await _userService.RegisterUserAsync(userRegisterDto, cancellationToken);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] UserPasswordChangeDto userPasswordChangeDto, CancellationToken cancellationToken)
    {
        var result = await _userService.ChangePasswordAsync(userPasswordChangeDto, cancellationToken);
        if (result.ResultStatus == ResultStatus.Error)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Message);
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