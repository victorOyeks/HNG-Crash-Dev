using API.Data;
using API.Dto;
using API.Entities;
using API.Interfaces;
using API.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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

            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error during user registration: {ex.Message}");
                return (false, "Registration unsuccessful", null, null);
            }
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


        public async Task<bool> UsersBelongToSameOrg(string userId1, string userId2)
        {
            var userOrganisations1 = await _userOrganisationRepository.GetUserOrganisationsAsync(userId1);
            var userOrganisations2 = await _userOrganisationRepository.GetUserOrganisationsAsync(userId2);

            if (userOrganisations1 == null || userOrganisations2 == null)
            {
                return false;
            }

            var organisationIds1 = userOrganisations1.Select(uo => uo.UserOrganisations);
            var organisationIds2 = userOrganisations2.Select(uo => uo.UserOrganisations);


            var commonOrganisationIds = organisationIds1.Intersect(organisationIds2);

            return commonOrganisationIds.Any();
        }
    }
}
