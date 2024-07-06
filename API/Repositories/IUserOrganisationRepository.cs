using API.Entities;

namespace API.Repositories
{
    public interface IUserOrganisationRepository
    {
        Task<UserOrganisation> AddUserOrganisationAsync(UserOrganisation userOrganisation);
        Task<bool> UserExistsInOrganisationAsync(string userId, string organisationId);
    }
}
