using API.Services;
using Microsoft.AspNetCore.Mvc;
using API.Dto;
using System.Diagnostics;

namespace API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var result = await _userService.RegisterAsync(request);

            if (!result.IsSuccess)
            {
                if (result.ErrorMessage == "Email already exists")
                {
                    return BadRequest(new { status = "Bad request", message = "Email already exists", statusCode = 400 });
                }
                else
                {
                    return BadRequest(new { status = "Bad request", message = "Registration unsuccessful", statusCode = 400 });
                }
            }

            return Created("", new
            {
                status = "success",
                message = "Registration successful",
                data = new
                {
                    accessToken = result.Token,
                    user = new
                    {
                        UserId = result.AppUser.UserId,
                        FirstName = result.AppUser.FirstName,
                        LastName = result.AppUser.LastName,
                        Email = result.AppUser.Email,
                        Phone = result.AppUser.Phone
                    }
                }
            });
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var result = await _userService.LoginAsync(request);
            if (!result.IsSuccess)
            {
                return Unauthorized(new { status = "Bad request", message = "Authentication failed", statusCode = 401 });
            }

            return Ok(new
            {
                status = "success",
                message = "Login successful",
                data = new
                {
                    accessToken = result.Token,
                    user = new
                    {
                        result.AppUser.UserId,
                        result.AppUser.FirstName,
                        result.AppUser.LastName,
                        result.AppUser.Email,
                        result.AppUser.Phone
                    }
                }
            });
        }
    }
}