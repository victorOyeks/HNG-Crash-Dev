using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "The FirstName field is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The LastName field is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "The Email field is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The Password field is required")]
        public string Password { get; set; }

        public string Phone { get; set; }
    }
}
