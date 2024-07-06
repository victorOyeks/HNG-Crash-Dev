using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserOrganisationRepository : IUserOrganisationRepository
    {
        private readonly AppDbContext _context;

        public UserOrganisationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserOrganisation> AddUserOrganisationAsync(UserOrganisation userOrganisation)
        {
            await _context.UserOrganisations.AddAsync(userOrganisation);
            await _context.SaveChangesAsync();
            return userOrganisation;
        }

        public async Task<bool> UserExistsInOrganisationAsync(string userId, string organisationId)
        {
            return await _context.UserOrganisations.AnyAsync(uo => uo.UserId == userId && uo.OrganisationId == organisationId);
        }
    }
}
