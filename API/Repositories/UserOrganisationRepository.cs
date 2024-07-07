using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserOrganisationRepository : IUserOrganisationRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;

        public UserOrganisationRepository(AppDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public async Task<UserOrganisation> AddUserOrganisationAsync(UserOrganisation userOrganisation)
        {
            await _context.UserOrganisations.AddAsync(userOrganisation);
            await _context.SaveChangesAsync();
            return userOrganisation;
        }


        public async Task<IEnumerable<Organisation>> GetUserOrganisationsAsync(string userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            return user?.UserOrganisations.Select(uo => uo.Organisation) ?? Enumerable.Empty<Organisation>();
        }

        public async Task<bool> UserExistsInOrganisationAsync(string userId, string organisationId)
        {
            return await _context.UserOrganisations.AnyAsync(uo => uo.UserId == userId && uo.OrganisationId == organisationId);
        }

        public async Task<UserOrganisation> GetUserOrganisationAsync(string userId, string organisationId)
        {
            return await _context.UserOrganisations
                .Include(uo => uo.AppUser)
                .Include(uo => uo.Organisation)
                .FirstOrDefaultAsync(uo => uo.UserId == userId && uo.OrganisationId == organisationId);
        }
    }
}
