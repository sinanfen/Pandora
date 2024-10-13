using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Pandora.Application.Utilities.Results;
using Pandora.Shared.DTOs.UserDTOs;
using Pandora.Shared.Enums;

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
    public async Task<IActionResult> CreateDefaultUser(CancellationToken cancellationToken)
    {
        var individualUserRegisterDto = new IndividualUserRegisterDto()
        {
            Username = "sinanfen",
            Email = "sinanfen@example.com",
            Password = "SinanFen123#",
            ConfirmPassword = "SinanFen123#",
            PhoneNumber = "123-456-7890",
            UserType = UserType.Individual,
            FirstName = "Sinan",
            LastName = "Fen"
        };

        var result = await _userService.RegisterUserAsync(individualUserRegisterDto, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
    {
        var data = await _userService.GetAllAsync(cancellationToken);
        return Ok(data);
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAsync(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.GetByIdAsync(userId, cancellationToken);
        if (result == null)
        {
            return NotFound();
        }
        return Ok(result);
    }

    // PUT: api/users/individual/{userId}
    [HttpPut("individual/{userId}")]
    public async Task<IActionResult> UpdateIndividualAsync(Guid userId, [FromBody] IndividualUserUpdateDto individualDto, CancellationToken cancellationToken)
    {
        if (userId != individualDto.Id)
        {
            return BadRequest("User ID in the URL does not match the ID in the request body.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.UpdateIndividualUserAsync(individualDto, cancellationToken);
        if (result == null)
        {
            return NotFound("User not found.");
        }

        return Ok(result);
    }

    // PUT: api/users/corporate/{userId}
    [HttpPut("corporate/{userId}")]
    public async Task<IActionResult> UpdateCorporateAsync(Guid userId, [FromBody] CorporateUserUpdateDto corporateDto, CancellationToken cancellationToken)
    {
        if (userId != corporateDto.Id)
        {
            return BadRequest("User ID in the URL does not match the ID in the request body.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _userService.UpdateCorporateUserAsync(corporateDto, cancellationToken);
        if (result == null)
        {
            return NotFound("User not found.");
        }

        return Ok(result);
    }

    // DELETE: api/users/delete/{userId}
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var result = await _userService.DeleteAsync(userId, cancellationToken);
        if (result.ResultStatus != ResultStatus.Success)
        {
            return NotFound();
        }

        return Ok(result);
    }
}
