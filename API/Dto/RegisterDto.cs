using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "First name is required")] 
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email name is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")] 
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        public string Phone { get; set; }
    }
}
