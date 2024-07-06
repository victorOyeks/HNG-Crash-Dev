using API.Data;
using API.Dto;
using API.Entities;
using API.Interfaces;
using API.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganisationService _organisationService;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly IUserOrganisationRepository _userOrganisationRepository;

        public UserService(IUserRepository userRepository, IOrganisationService organisationService, IConfiguration configuration, ITokenService tokenService, IUserOrganisationRepository userOrganisationRepository)
        {
            _userRepository = userRepository;
            _organisationService = organisationService;
            _configuration = configuration;
            _tokenService = tokenService;
            _userOrganisationRepository = userOrganisationRepository;
        }

        public async Task<(bool IsSuccess, string ErrorMessage, AppUser AppUser, string Token)> RegisterAsync(RegisterDto request)
        {
            if (await _userRepository.UserExistsAsync(request.Email))
            {
                return (false, "Email already exists", null, null);
            }
            var user = new AppUser
            {

                UserId = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Phone = request.Phone,
            };

            await _userRepository.AddUserAsync(user);

            var organisation = await _organisationService.CreateOrganisationAsync(user.UserId, request.FirstName);

            var userOrganisation = new UserOrganisation
            {
                AppUser = user,
                Organisation = organisation,
            };

            await _userOrganisationRepository.AddUserOrganisationAsync(userOrganisation);

            var token = _tokenService.CreateToken(user);

            return (true, null, user, token);
        }

        public async Task<(bool IsSuccess, string ErrorMessage, AppUser AppUser, string Token)> LoginAsync(LoginDto  request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return (false, "Invalid credentials", null, null);
            }

            var token = _tokenService.CreateToken(user);

            return (true, null, user, token);
        }

        public async Task<AppUser> GetAppUserAsync(string userId)
        {
            return await _userRepository.GetUserByIdAsync(userId);
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }
    }
}
