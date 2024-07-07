using API.Entities;
using API.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Services
{
    public class OrganisationService : IOrganisationService
    {
        private readonly IOrganisationRepository _organisationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserOrganisationRepository _userOrganisationRepository;

        public OrganisationService(IOrganisationRepository organisationRepository, IUserRepository userRepository, IUserOrganisationRepository userOrganisationRepository)
        {
            _organisationRepository = organisationRepository;
            _userRepository = userRepository;
            _userOrganisationRepository = userOrganisationRepository;
        }

        public async Task<Organisation> CreateOrganisationAsync(string userId, string firstName)
        {
            var organisation = new Organisation
            {
                OrgId = Guid.NewGuid().ToString(),
                Name = $"{firstName}'s Organisation",
                Description = ""
            };

            await _organisationRepository.AddOrganisationAsync(organisation);
            return organisation;
        }

        public async Task<Organisation> CreateOrganisationAsync(string name, string description, string userId)
        {
            var organisation = new Organisation
            {
                OrgId = Guid.NewGuid().ToString(),
                Name = name,
                Description = description,
            };

            await _organisationRepository.AddOrganisationAsync(organisation);
            await AddUserToOrganisationAsync(userId, organisation.OrgId);

            return organisation;
        }

        public async Task<Organisation> GetOrganisationByIdAsync(string id)
        {
            return await _organisationRepository.GetOrganisationByIdAsync(id);
        }

        public async Task<IEnumerable<Organisation>> GetUserOrganisationsAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user?.UserOrganisations.Select(uo => uo.Organisation) ?? Enumerable.Empty<Organisation>();
        }

        public async Task AddUserToOrganisationAsync(string userId, string organisationId)
        {
            var organisation = await _organisationRepository.GetOrganisationByIdAsync(organisationId);
            if (organisation == null)
            {
                throw new ApplicationException("Organisation not found.");
            }

            await _organisationRepository.AddUserToOrganisationAsync(userId, organisationId);
        }


        public async Task<UserOrganisation> GetUserOrganisationAsync(string userId, string organisationId)
        {
            return await _userOrganisationRepository.GetUserOrganisationAsync(userId, organisationId);
        }
    }
}
