using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Results;
using Pandora.Shared.DTOs.PersonalVaultDTOs;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Pandora.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PersonalVaultsController : ControllerBase
{
    private readonly IPersonalVaultService _personalVaultService;

    public PersonalVaultsController(IPersonalVaultService personalVaultService)
    {
        _personalVaultService = personalVaultService;   
    }

    /// <summary>
    /// Retrieves the currently logged-in user's ID from the JWT token.
    /// </summary>
    private Guid GetLoggedInUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID could not be found.");
        return Guid.Parse(userIdClaim.Value);
    }

    /// <summary>
    /// Retrieves a personal vault entry by its ID for the authenticated user.
    /// </summary>
    [HttpGet("{personalVaultId}")]
    [SwaggerOperation(Summary = "Get personal vault by ID", Description = "Fetches a specific personal vault entry that belongs to the logged-in user.")]
    [SwaggerResponse(200, "Personal vault entry found")]
    [SwaggerResponse(204, "No content available")]
    [SwaggerResponse(500, "Internal server error")]
    public async Task<IActionResult> GetByIdAsync(Guid personalVaultId, CancellationToken cancellationToken)
    {
        // Kullanıcının kimliğini al
        var userId = GetLoggedInUserId();
        var result = await _personalVaultService.GetByIdAndUserAsync(personalVaultId, userId, cancellationToken);
        return result.ResultStatus switch
        {
            ResultStatus.Success => Ok(result.Data),
            ResultStatus.Warning => NoContent(),
            ResultStatus.Error => StatusCode(500, result.Message),
            _ => BadRequest(result.Message)
        };
    }

    /// <summary>
    /// Retrieves all personal vault entries for the logged-in user.
    /// </summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all personal vaults", Description = "Fetches all personal vault entries for the authenticated user.")]
    [SwaggerResponse(200, "Personal vault entries retrieved")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();
        var personalVaults = await _personalVaultService.GetAllByUserAsync(userId, cancellationToken);
        return Ok(personalVaults);
    }

    /// <summary>
    /// Adds a new personal vault entry.
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Add a personal vault entry", Description = "Creates a new personal vault entry for the logged-in user.")]
    [SwaggerResponse(200, "Personal vault entry created")]
    [SwaggerResponse(400, "Invalid input or creation failed")]
    public async Task<IActionResult> AddAsync([FromBody] PersonalVaultAddDto dto, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId(); // JWT token'dan kullanıcı ID'sini al
        var result = await _personalVaultService.AddAsync(dto, userId, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });
        return Ok(result.Data);
    }

    /// <summary>
    /// Updates an existing personal vault entry.
    /// </summary>
    [HttpPut]
    [SwaggerOperation(Summary = "Update a personal vault entry", Description = "Updates a personal vault entry if it belongs to the authenticated user.")]
    [SwaggerResponse(200, "Personal vault entry updated")]
    [SwaggerResponse(400, "Invalid input or update failed")]
    [SwaggerResponse(401, "Unauthorized access")]
    public async Task<IActionResult> UpdateAsync([FromBody] PersonalVaultUpdateDto dto, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var result = await _personalVaultService.UpdateAsync(dto, userId, cancellationToken);
        if (result.ResultStatus == ResultStatus.Error)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });
        return Ok(result.Data);
    }

    /// <summary>
    /// Deletes a personal vault entry by ID.
    /// </summary>
    [HttpDelete("{personalVaultId}")]
    [SwaggerOperation(Summary = "Delete a personal vault entry", Description = "Deletes a personal vault entry if it belongs to the authenticated user.")]
    [SwaggerResponse(200, "Personal vault entry deleted")]
    [SwaggerResponse(401, "Unauthorized access")]
    public async Task<IActionResult> DeleteAsync(Guid personalVaultId, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var personalVault = await _personalVaultService.GetByIdAsync(personalVaultId, cancellationToken);
        if (personalVault == null || personalVault.UserId != userId)
            return Unauthorized("You are not authorized to take this action.");
        var result = await _personalVaultService.DeleteAsync(personalVaultId, cancellationToken);
        if (result.ResultStatus == ResultStatus.Error)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });
        return Ok(result.Message);
    }

    /// <summary>
    /// Generates a shareable link for a locked time capsule.
    /// </summary>
    [HttpPost("{personalVaultId}/generate-share-link")]
    [SwaggerOperation(Summary = "Generate share link for time capsule", Description = "Creates a shareable link for a locked time capsule. Only locked vaults can be shared.")]
    [SwaggerResponse(200, "Share link generated successfully")]
    [SwaggerResponse(400, "Invalid request or vault is not locked")]
    [SwaggerResponse(401, "Unauthorized access")]
    public async Task<IActionResult> GenerateShareLinkAsync(Guid personalVaultId, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();
        var result = await _personalVaultService.GenerateShareLinkAsync(personalVaultId, userId, cancellationToken);
        
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });
            
        return Ok(new { ShareLink = result.Data, Message = result.Message });
    }
}
