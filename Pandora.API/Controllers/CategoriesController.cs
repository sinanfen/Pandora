using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.DTOs.CategoryDTOs;
using Pandora.Application.Interfaces;
using Pandora.Application.Utilities.Results;

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

    // GET: api/categories/get/{categoryId}
    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetByIdAsync(Guid categoryId)
    {
        var cts = new CancellationTokenSource();
        var result = await _categoryService.GetByIdAsync(categoryId, cts.Token);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    // GET: api/categories/getall
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var cts = new CancellationTokenSource();
        var result = await _categoryService.GetAllAsync(cts.Token);
        return Ok(result);
    }

    // POST: api/categories/add
    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] CategoryAddDto categoryAddDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var cts = new CancellationTokenSource();
        var result = await _categoryService.AddAsync(categoryAddDto, cts.Token);
        return Ok(result); 
    }

    // PUT: api/categories/update/{categoryId}
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] CategoryUpdateDto categoryUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var cts = new CancellationTokenSource();
        var result = await _categoryService.UpdateAsync(categoryUpdateDto, cts.Token);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result); // 200 OK
    }

    // DELETE: api/categories/delete/{categoryId}
    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteAsync(Guid categoryId)
    {
        var cts = new CancellationTokenSource();
        var result = await _categoryService.DeleteAsync(categoryId, cts.Token);
        if (result.ResultStatus != ResultStatus.Success)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
