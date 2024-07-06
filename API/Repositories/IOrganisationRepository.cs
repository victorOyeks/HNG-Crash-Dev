using API.Entities;

namespace API.Repositories
{
    public interface IOrganisationRepository
    {
        Task AddOrganisationAsync(Organisation organisation);
        Task<Organisation> GetOrganisationByIdAsync(string orgId);
        Task AddUserToOrganisationAsync(string userId, string organisationId);
    }
}
