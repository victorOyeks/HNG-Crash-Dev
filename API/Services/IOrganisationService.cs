using API.Entities;

namespace API.Services
{
    public interface IOrganisationService
    {
        Task<Organisation> CreateOrganisationAsync(string userId, string firstName);
        Task<IEnumerable<Organisation>> GetUserOrganisationsAsync(string userId);
        Task<Organisation> GetOrganisationByIdAsync(string id);
        Task<Organisation> CreateOrganisationAsync(string name, string description, string userId);
        Task AddUserToOrganisationAsync(string userId, string organisationId);
    }
}
