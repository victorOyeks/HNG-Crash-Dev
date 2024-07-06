using API.Entities;

namespace API.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByEmailAsync(string email);
        Task AddUserAsync(AppUser user);
        Task<bool> UserExistsAsync(string email);
        Task<AppUser> GetUserByIdAsync(string userId);
    }
}
