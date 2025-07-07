using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.Interfaces.Results;
using Pandora.Shared.DTOs.UserDTOs;
using Swashbuckle.AspNetCore.Annotations;

namespace Pandora.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Creates a default admin user (for development/testing purposes).
    /// </summary>
    [HttpPost("default")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Create a default user", Description = "Creates a predefined default user. Typically used in development or testing.")]
    [SwaggerResponse(200, "Default user created successfully")]
    public async Task<IActionResult> CreateDefaultUser(CancellationToken cancellationToken)
    {
        UserRegisterDto? userRegisterDto = new UserRegisterDto()
        {
            Username = "sinanfen",
            Email = "sinanfen@example.com",
            Password = "SinanFen123#",
            ConfirmPassword = "SinanFen123#",
            PhoneNumber = "123-456-7890",
            FirstName = "Sinan",
            LastName = "Fen"
        };

        var result = await _userService.RegisterUserAsync(userRegisterDto, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all users (Admin only).
    /// </summary>
    [Authorize("Admin")]
    [HttpGet]
    [SwaggerOperation(Summary = "Get all users", Description = "Returns a list of all users. Only accessible by Admin role.")]
    [SwaggerResponse(200, "Users retrieved successfully")]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var data = await _userService.GetAllAsync(cancellationToken);
        return Ok(data);
    }

    /// <summary>
    /// Retrieves a user by ID.
    /// </summary>
    [HttpGet("{userId}")]
    [SwaggerOperation(Summary = "Get user by ID", Description = "Fetches a user's details using their unique ID.")]
    [SwaggerResponse(200, "User found")]
    [SwaggerResponse(404, "User not found")]
    public async Task<IActionResult> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.GetByIdAsync(userId, cancellationToken);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// Updates a user's profile information.
    /// </summary>
    [HttpPut("{userId}")]
    [SwaggerOperation(Summary = "Update user", Description = "Updates the user profile data based on user ID.")]
    [SwaggerResponse(200, "User updated successfully")]
    [SwaggerResponse(400, "Bad request - invalid model or ID mismatch")]
    [SwaggerResponse(404, "User not found")]
    public async Task<IActionResult> UpdateAsync(Guid userId, [FromBody] UserUpdateDto userUpdateDto, CancellationToken cancellationToken)
    {
        if (userId != userUpdateDto.Id)
            return BadRequest("User ID in the URL does not match the ID in the request body.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.UpdateAsync(userUpdateDto, cancellationToken);
        if (result == null)
            return NotFound("User not found.");

        return Ok(result);
    }

    /// <summary>
    /// Deletes a user by ID (Admin only).
    /// </summary>
    [Authorize("Admin")]
    [HttpDelete("{userId}")]
    [SwaggerOperation(Summary = "Delete user", Description = "Deletes a user by ID. Only accessible by Admins.")]
    [SwaggerResponse(200, "User deleted successfully")]
    [SwaggerResponse(404, "User not found")]
    public async Task<IActionResult> DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.DeleteAsync(userId, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
            return NotFound();

        return Ok(result);
    }
}
