using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.DTOs.UserDTOs;
using Pandora.Core.Domain.Constants.Enums;
using Microsoft.AspNetCore.Authorization;

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
    [HttpGet("CreateDefaultUser")]
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

    [HttpPost]
    public async Task<IActionResult> RegisterAsync(UserRegisterDto userRegisterDto)
    {
        var cts = new CancellationTokenSource();
        var result = await _userService.RegisterUserAsync(userRegisterDto, cts.Token);
        return Ok(result);
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllAsync()
    {
        var cts = new CancellationTokenSource();
        var data = await _userService.GetAllAsync(cts.Token);
        return Ok(data);
    }
}
