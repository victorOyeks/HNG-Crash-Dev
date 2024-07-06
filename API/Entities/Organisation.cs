using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class Organisation
    {
        public Organisation() { }

        [Key]
        public string OrgId { get; set; }
        [Required]
        public string Name { get; set; }
        public string  Description { get; set; }
        public ICollection<UserOrganisation> UserOrganisations { get; set; } = new List<UserOrganisation>();
    }
}
