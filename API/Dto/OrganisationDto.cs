using System.ComponentModel.DataAnnotations;

namespace API.Dto
{
    public class OrganisationDto
    {
        [Required] 
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
