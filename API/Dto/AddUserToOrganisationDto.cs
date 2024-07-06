using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class AddUserToOrganisationDto
    {
        [Required]
        public string UserId { get; set; }
    }
}