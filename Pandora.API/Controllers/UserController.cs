using Microsoft.AspNetCore.Mvc;
using Pandora.Application.Interfaces;
using Pandora.Application.DTOs.UserDTOs;
using Pandora.Core.Domain.Constants.Enums;

namespace Pandora.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
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

            var result = await _userService.RegisterUserAsync(userRegisterDto);

            return Ok(result);
        }
    }
}
