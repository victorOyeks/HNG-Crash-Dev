
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class AppUser 
    {
        [Key]
        public string UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Phone { get; set; }
        public ICollection<UserOrganisation> UserOrganisations { get; set; } = new List<UserOrganisation>();
    }
}
