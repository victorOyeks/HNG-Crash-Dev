using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class UserDto
    {
        [Required] public string UserId { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required] public string Email { get; set; }
        [Required] public string Phone { get; set; }
    }
}
