using API.Dto;
using API.Entities;

namespace API.Services
{
    public interface IUserService
    {
        Task<(bool IsSuccess, string ErrorMessage, AppUser AppUser, string Token)> RegisterAsync(RegisterDto request);
        Task<(bool IsSuccess, string ErrorMessage, AppUser AppUser, string Token)> LoginAsync(LoginDto request);
        Task<AppUser> GetUserByIdAsync(string id);
    }
}
