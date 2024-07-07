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
            var currentUserId = User.FindFirst("userId")?.Value;

            var requestedUser = await _userService.GetUserByIdAsync(id);
            if (requestedUser == null)
            {
                return NotFound(new
                {
                    status = "Not Found",
                    message = "User not found",
                    StatusCode = 404
                });
            }

            var currentUser = await _userService.GetUserByIdAsync(currentUserId);
            var isCurrentUser = requestedUser.UserId == currentUserId;

            var usersBelongToSameOrg = await _userService.UsersBelongToSameOrg(currentUserId, id);

            if (!isCurrentUser && !usersBelongToSameOrg)
            {
                return Forbid();
            }

            return Ok(new
            {
                status = "success",
                message = "User record retrieved successfully",
                data = new
                {
                    requestedUser.UserId,
                    requestedUser.FirstName,
                    requestedUser.LastName,
                    requestedUser.Email,
                    requestedUser.Phone
                }
            });
        }
    }
}
