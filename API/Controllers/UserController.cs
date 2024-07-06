using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(new
                {
                    status = "Not Found",
                    message = "User not found",
                    StatusCode = 404
                });
            }

            return Ok(new
            {
                status = "success",
                message = "User record retrieved successfully",
                data = new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.Phone
                }
            });
        }
    }
}
