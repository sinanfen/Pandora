using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.DTOs.PersonalVaultDTOs;
using Pandora.Application.Interfaces;
using Pandora.Application.Security.Interfaces;
using Pandora.Application.Utilities.Results;
using System.Security.Claims;

namespace Pandora.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PersonalVaultsController : ControllerBase
{
    private readonly IPersonalVaultService _personalVaultService;
    private readonly IHasher _hasher;
    private readonly IEncryption _encryption;

    public PersonalVaultsController(IPersonalVaultService personalVaultService, IHasher hasher, IEncryption encryption)
    {
        _personalVaultService = personalVaultService;
        _hasher = hasher;
        _encryption = encryption;
    }

    private Guid GetLoggedInUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");

        return Guid.Parse(userIdClaim.Value);
    }

    // GET: api/PersonalVaults/{personalVaultId}
    [HttpGet("{personalVaultId}")]
    public async Task<IActionResult> GetByIdAsync(Guid personalVaultId, CancellationToken cancellationToken)
    {
        // Kullanıcının kimliğini al
        var userId = GetLoggedInUserId();

        var personalVault = await _personalVaultService.GetByIdAndUserAsync(personalVaultId, userId, cancellationToken);
        if (personalVault == null)
            return NotFound("Personal vault bulunamadı.");

        return Ok(personalVault);
    }

    // GET: api/PersonalVaults/getall
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();

        var vaults = await _personalVaultService.GetAllByUserAsync(userId, cancellationToken);
        return Ok(vaults);
    }

    // POST: api/PersonalVaults/add
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] PersonalVaultAddDto dto, CancellationToken cancellationToken)
    {
        var result = await _personalVaultService.AddAsync(dto, cancellationToken);
        if (result.ResultStatus == ResultStatus.Error)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    // PUT: api/PersonalVaults/update
    [HttpPut]
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] PersonalVaultUpdateDto dto, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var personalVault = await _personalVaultService.GetByIdAsync(dto.Id, cancellationToken);

        if (personalVault == null || personalVault.UserId != userId)
            return Unauthorized("Bu kişisel vault güncellenemez.");

        var result = await _personalVaultService.UpdateAsync(dto, cancellationToken);
        if (result.ResultStatus == ResultStatus.Error)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }


    // DELETE: api/PersonalVaults/{personalVaultId}
    [HttpDelete("{personalVaultId}")]
    public async Task<IActionResult> DeleteAsync(Guid personalVaultId, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var personalVault = await _personalVaultService.GetByIdAsync(personalVaultId, cancellationToken);

        if (personalVault == null || personalVault.UserId != userId)
            return Unauthorized("Bu kişisel vault silinemez.");

        var result = await _personalVaultService.DeleteAsync(personalVaultId, cancellationToken);
        if (result.ResultStatus == ResultStatus.Error)
            return NotFound(result.Message);

        return Ok(result.Message);
    }

}
