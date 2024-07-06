using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(AppUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<AppUser> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await _context.Users.Include(u => u.UserOrganisations).ThenInclude(uo => uo.Organisation)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            Console.WriteLine("--------18");
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
    }
}
