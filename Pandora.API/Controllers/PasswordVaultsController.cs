using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.Utilities.Results;
using Pandora.Shared.DTOs.PasswordVaultDTOs;
using System.Security.Claims;

namespace Pandora.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PasswordVaultsController : ControllerBase
{
    private readonly IPasswordVaultService _passwordVaultService;

    public PasswordVaultsController(IPasswordVaultService passwordVaultService)
    {
        _passwordVaultService = passwordVaultService;
    }

    private Guid GetLoggedInUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");

        return Guid.Parse(userIdClaim.Value);
    }

    // GET: api/PasswordVaults/{passwordVaultId}
    [HttpGet("{passwordVaultId}")]
    public async Task<IActionResult> GetByIdAsync(Guid passwordVaultId, CancellationToken cancellationToken)
    {
        // Kullanıcının kimliğini al
        var userId = GetLoggedInUserId();

        var passwordVault = await _passwordVaultService.GetByIdAndUserAsync(passwordVaultId, userId, cancellationToken);
        if (passwordVault == null)
            return NotFound("Kişisel kasa bulunamadı.");

        return Ok(passwordVault);
    }

    // GET: api/PasswordVaults/getall
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();
        var vaults = await _passwordVaultService.GetAllByUserAsync(userId, cancellationToken);
        return Ok(vaults);
    }

    // POST: api/PasswordVaults/add
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] PasswordVaultAddDto dto, CancellationToken cancellationToken)
    {
        var result = await _passwordVaultService.AddAsync(dto, cancellationToken);
        if (result.ResultStatus == ResultStatus.Error)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    // PUT: api/PasswordVaults/update
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] PasswordVaultUpdateDto dto, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var passwordVault = await _passwordVaultService.GetByIdAsync(dto.Id, cancellationToken);

        if (passwordVault == null || passwordVault.UserId != userId)
            return Unauthorized("Bu şifre kasası güncellenemez.");

        var result = await _passwordVaultService.UpdateAsync(dto, cancellationToken);
        if (result.ResultStatus == ResultStatus.Error)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    // DELETE: api/PasswordVaults/{passwordVaultId}
    [HttpDelete("{passwordVaultId}")]
    public async Task<IActionResult> DeleteAsync(Guid passwordVaultId, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var passwordVault = await _passwordVaultService.GetByIdAsync(passwordVaultId, cancellationToken);

        if (passwordVault == null || passwordVault.UserId != userId)
            return Unauthorized("Bu şifre kasası silinemez.");

        var result = await _passwordVaultService.DeleteAsync(passwordVaultId, cancellationToken);
        if (result.ResultStatus == ResultStatus.Error)
            return NotFound(result.Message);

        return Ok(result.Message);
    }
}
