using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly AppDbContext _context;

        public OrganisationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOrganisationAsync(Organisation organisation)
        {
            Console.WriteLine("ORGANISATION SAVED-----------------------------------------");
            await _context.Organisations.AddAsync(organisation);
            await _context.SaveChangesAsync();
        }

        public async Task AddUserToOrganisationAsync(string userId, string organisationId)
        {
            var userOrganisation = new UserOrganisation
            {
                UserId = userId,
                OrganisationId = organisationId
            };

            await _context.UserOrganisations.AddAsync(userOrganisation);
            await _context.SaveChangesAsync();
        }

        public async Task<Organisation> GetOrganisationByIdAsync(string orgId)
        {
            return await _context.Organisations.FirstOrDefaultAsync(o => o.OrgId == orgId);
        }
    }
}
