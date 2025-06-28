using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Results;
using Pandora.Shared.DTOs.PasswordVaultDTOs;
using Swashbuckle.AspNetCore.Annotations;
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

    /// <summary>
    /// Retrieves the currently logged-in user's ID from JWT claims.
    /// </summary>
    private Guid GetLoggedInUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID not found.");
        return Guid.Parse(userIdClaim.Value);
    }

    /// <summary>
    /// Retrieves a specific password vault by its ID for the logged-in user.
    /// </summary>
    [HttpGet("{passwordVaultId}")]
    [SwaggerOperation(Summary = "Get password vault by ID", Description = "Fetches a specific password vault if it belongs to the authenticated user.")]
    [SwaggerResponse(200, "Password vault found")]
    [SwaggerResponse(204, "No content for the provided ID")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetByIdAsync(Guid passwordVaultId, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();
        var result = await _passwordVaultService.GetByIdAndUserAsync(passwordVaultId, userId, cancellationToken);
        return result.ResultStatus switch
        {
            ResultStatus.Success => Ok(result.Data),
            ResultStatus.Warning => NoContent(),
            ResultStatus.Error => StatusCode(500, result.Message),
            _ => BadRequest(result.Message)
        };
    }

    /// <summary>
    /// Retrieves all password vault entries for the logged-in user.
    /// </summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all password vaults", Description = "Fetches all password vault entries belonging to the authenticated user.")]
    [SwaggerResponse(200, "Password vaults retrieved")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();
        var passwordVaultDtos = await _passwordVaultService.GetAllByUserAsync(userId, cancellationToken);
        return Ok(passwordVaultDtos);
    }

    /// <summary>
    /// Adds a new password vault entry.
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Add a password vault entry", Description = "Creates a new password vault entry for the authenticated user.")]
    [SwaggerResponse(200, "Password vault created")]
    [SwaggerResponse(400, "Invalid input or creation failed")]
    public async Task<IActionResult> AddAsync([FromBody] PasswordVaultAddDto dto, CancellationToken cancellationToken)
    {
        var result = await _passwordVaultService.AddAsync(dto, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });
        return Ok(result.Data);
    }

    /// <summary>
    /// Updates an existing password vault entry.
    /// </summary>
    [HttpPut]
    [SwaggerOperation(Summary = "Update a password vault entry", Description = "Updates an existing password vault entry if it belongs to the authenticated user.")]
    [SwaggerResponse(200, "Password vault updated")]
    [SwaggerResponse(400, "Invalid input or update failed")]
    [SwaggerResponse(401, "Unauthorized access")]
    public async Task<IActionResult> UpdateAsync([FromBody] PasswordVaultUpdateDto dto, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var passwordVault = await _passwordVaultService.GetByIdAsync(dto.Id, cancellationToken);
        if (passwordVault == null || passwordVault.UserId != userId)
            return Unauthorized("You are not authorized to take this action.");
        var result = await _passwordVaultService.UpdateAsync(dto, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });
        return Ok(result.Data);
    }

    /// <summary>
    /// Deletes a password vault entry by ID.
    /// </summary>
    [HttpDelete("{passwordVaultId}")]
    [SwaggerOperation(Summary = "Delete a password vault entry", Description = "Deletes a password vault entry if it belongs to the authenticated user.")]
    [SwaggerResponse(200, "Password vault deleted")]
    [SwaggerResponse(401, "Unauthorized access")]
    public async Task<IActionResult> DeleteAsync(Guid passwordVaultId, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var passwordVault = await _passwordVaultService.GetByIdAsync(passwordVaultId, cancellationToken);
        if (passwordVault == null || passwordVault.UserId != userId)
            return Unauthorized("You are not authorized to take this action.");
        var result = await _passwordVaultService.DeleteAsync(passwordVaultId, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });
        return Ok(result.Message);
    }
}
