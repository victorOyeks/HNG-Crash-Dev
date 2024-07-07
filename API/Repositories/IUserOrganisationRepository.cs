using API.Entities;

namespace API.Repositories
{
    public interface IUserOrganisationRepository
    {
        Task<UserOrganisation> AddUserOrganisationAsync(UserOrganisation userOrganisation);
        Task<bool> UserExistsInOrganisationAsync(string userId, string organisationId);
        Task<IEnumerable<Organisation>> GetUserOrganisationsAsync(string userId);
        Task<UserOrganisation> GetUserOrganisationAsync(string userId, string organisationId);

    }
}
