using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.DTOs.UserDTOs;
using Pandora.Core.Domain.Constants.Enums;
using Microsoft.AspNetCore.Authorization;
using Pandora.Application.Utilities.Results;

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

    // GET: api/User/CreateDefaultUser
    [HttpPost("default")]
    public async Task<IActionResult> CreateDefaultUser()
    {
        var userRegisterDto = new UserRegisterDto
        {
            Username = "sinanfen",
            Email = "sinanfen@example.com",
            Password = "SinanFen123#",
            ConfirmPassword = "SinanFen123#",
            PhoneNumber = "123-456-7890",
            UserType = UserType.Individual,
            FirstName = "Test",
            LastName = "User"
        };

        var cts = new CancellationTokenSource();
        var result = await _userService.RegisterUserAsync(userRegisterDto, cts.Token);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var cts = new CancellationTokenSource();
        var data = await _userService.GetAllAsync(cts.Token);
        return Ok(data);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAsync(Guid userId)
    {
        var cts = new CancellationTokenSource();
        var result = await _userService.GetByIdAsync(userId, cts.Token);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    // PUT: api/users/update/{userId}
    [HttpPut]
    public async Task<IActionResult> UpdateAsync([FromBody] UserUpdateDto userUpdateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var cts = new CancellationTokenSource();
        var result = await _userService.UpdateUserAsync(userUpdateDto, cts.Token);
        if (result == null)
        {
            return NotFound();
        }

        return Ok(result); // 200 OK
    }

    // DELETE: api/users/delete/{userId}
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteAsync(Guid userId)
    {
        var cts = new CancellationTokenSource();
        var result = await _userService.DeleteAsync(userId, cts.Token);
        if (result.ResultStatus != ResultStatus.Success)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
