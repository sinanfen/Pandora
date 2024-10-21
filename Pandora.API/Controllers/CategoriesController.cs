using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.Utilities.Results;
using Pandora.Shared.DTOs.CategoryDTOs;
using System.Security.Claims;

namespace Pandora.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    private Guid GetLoggedInUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("Kullanıcı kimliği bulunamadı.");

        return Guid.Parse(userIdClaim.Value);
    }

    // GET: api/categories/get/{categoryId}
    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();

        var result = await _categoryService.GetByIdAndUserAsync(categoryId, userId, cancellationToken);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    // GET: api/categories/getall
    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();

        var result = await _categoryService.GetAllByUserAsync(userId, cancellationToken);
        return Ok(result);
    }

    // POST: api/categories/add
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CategoryAddDto categoryAddDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.AddAsync(categoryAddDto, cancellationToken);
        return Ok(result);
    }

    // PUT: api/categories/update/{categoryId}
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var categoryDto = await _categoryService.GetByIdAsync(categoryUpdateDto.Id, cancellationToken);

        if (categoryDto == null || categoryDto.UserId != userId)
            return Unauthorized("Bu kategori güncellenemez.");

        var result = await _categoryService.UpdateAsync(categoryUpdateDto, cancellationToken);
        if (result == null)
            return NotFound();

        return Ok(result); // 200 OK
    }

    // DELETE: api/categories/delete/{categoryId}
    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var categoryDto = await _categoryService.GetByIdAsync(categoryId, cancellationToken);

        if (categoryDto == null || categoryDto.UserId != userId)
            return Unauthorized("Bu kategori silinemez.");

        var result = await _categoryService.DeleteAsync(categoryId, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return NotFound();

        return Ok(result);
    }
}
