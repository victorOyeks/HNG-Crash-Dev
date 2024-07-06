/*using API.Dto;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;

        public AccountController(ITokenService tokenService, SignInManager<AppUser> signInManager, IConfiguration config)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginDto loginDto)
        {
            var userName = _config["UserCredentials:Email"];
            var password = _config["UserCredentials:Password"];

            if ((loginDto.Email != userName) || (loginDto.Password != password)) return BadRequest("Username or Password is not correct");

            var user = new AppUser
            {
                Email = userName,
                PasswordHash = password
            };

            return Ok(
                new LoginResponse
                {
                    UserName = userName,
                    Token = _tokenService.CreateToken(user)
                });
        }
    }
}
*/
