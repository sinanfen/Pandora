using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Results;
using Pandora.Shared.DTOs.CategoryDTOs;
using Swashbuckle.AspNetCore.Annotations;
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

    /// <summary>
    /// Returns the currently authenticated user's ID from the JWT token.
    /// </summary>
    private Guid GetLoggedInUserId()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            throw new UnauthorizedAccessException("User ID not found.");
        return Guid.Parse(userIdClaim.Value);
    }

    /// <summary>
    /// Retrieves a category by ID for the logged-in user.
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    /// <returns>Category details</returns>
    [HttpGet("{categoryId}")]
    [SwaggerOperation(Summary = "Get category by ID", Description = "Retrieves a specific category that belongs to the authenticated user.")]
    [SwaggerResponse(200, "Category found")]
    [SwaggerResponse(404, "Category not found")]
    public async Task<IActionResult> GetByIdAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();
        var categoryDto = await _categoryService.GetByIdAndUserAsync(categoryId, userId, cancellationToken);
        if (categoryDto == null)
            return NotFound();
        return Ok(categoryDto);
    }

    /// <summary>
    /// Retrieves all categories for the logged-in user.
    /// </summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Get all categories", Description = "Retrieves all categories for the authenticated user.")]
    [SwaggerResponse(200, "Categories retrieved")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId();
        var categoryDtos = await _categoryService.GetAllByUserAsync(userId, cancellationToken);
        return Ok(categoryDtos);
    }

    /// <summary>
    /// Creates a new category.
    /// </summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Add a new category", Description = "Creates a new category for the authenticated user.")]
    [SwaggerResponse(200, "Category created successfully")]
    [SwaggerResponse(400, "Invalid input")]
    public async Task<IActionResult> AddAsync([FromBody] CategoryAddDto categoryAddDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);
        var result = await _categoryService.AddAsync(categoryAddDto, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing category.
    /// </summary>
    [HttpPut]
    [SwaggerOperation(Summary = "Update a category", Description = "Updates an existing category if it belongs to the authenticated user.")]
    [SwaggerResponse(200, "Category updated")]
    [SwaggerResponse(400, "Invalid input")]
    [SwaggerResponse(401, "Unauthorized access")]
    public async Task<IActionResult> UpdateAsync([FromBody] CategoryUpdateDto categoryUpdateDto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);
        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var categoryDto = await _categoryService.GetByIdAsync(categoryUpdateDto.Id, cancellationToken);
        if (categoryDto == null || categoryDto.UserId != userId)
            return Unauthorized("This category cannot be updated.");
        var result = await _categoryService.UpdateAsync(categoryUpdateDto, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });
        return Ok(result); // 200 OK
    }

    /// <summary>
    /// Deletes a category by ID.
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    [HttpDelete("{categoryId}")]
    [SwaggerOperation(Summary = "Delete a category", Description = "Deletes a category if it belongs to the authenticated user.")]
    [SwaggerResponse(200, "Category deleted")]
    [SwaggerResponse(401, "Unauthorized access")]
    public async Task<IActionResult> DeleteAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        var userId = GetLoggedInUserId(); // JWT'den kullanıcı kimliği al
        var categoryDto = await _categoryService.GetByIdAsync(categoryId, cancellationToken);
        if (categoryDto == null || categoryDto.UserId != userId)
            return Unauthorized("This category cannot be deleted.");
        var result = await _categoryService.DeleteAsync(categoryId, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return BadRequest(new { Result = result.ResultStatus, Message = result.Message });
        return Ok(result);
    }
}
