namespace API.Entities
{
    public class UserOrganisation
    {
        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

        public string OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
    }
}
