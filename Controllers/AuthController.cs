using Microsoft.AspNetCore.Mvc;
using CropDeal.Models;
using CropDeal.Services; 

namespace CropDeal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request )
        {
            var result = await _userService.RegisterUserAsync(request);
            return Ok(new { message = result });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var token = await _userService.LoginAsync(request);

            if (token == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            return Ok(new { Token = token });
        }
    }
}