using API.Extensions;
using API.Dto;
using API.Entities;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.Repositories;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/organisations")]
    public class OrganisationController : ControllerBase
    {
        private readonly IOrganisationService _organisationService;
        private readonly IUserService _userService;
        private readonly IUserOrganisationRepository _userOrganisationRepository;

        public OrganisationController(IOrganisationService organisationService, IUserService userService, IUserOrganisationRepository userOrganisationRepository)
        {
            _organisationService = organisationService;
            _userService = userService;
            _userOrganisationRepository = userOrganisationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrganisations()
        {
            var username = User.GetUsername();
            var userId = User.FindFirst("userId")?.Value;

            var organisations = await _organisationService.GetUserOrganisationsAsync(userId);
            return Ok(new
            {
                status = "success",
                message = "Organisations retrieved successfully",
                data = new
                {
                    organisations = organisations.Select(o => new
                    {
                        o.OrgId,
                        o.Name,
                        o.Description,
                    }).ToList()
                }
            });
        }

        [HttpGet("{orgId}")]
        public async Task<IActionResult> GetOrganisationById(string orgId)
        {
            var userId = User.FindFirst("userId")?.Value;
            var organisation = await _organisationService.GetOrganisationByIdAsync(orgId);
            if (organisation == null)
            {
                return NotFound(new
                {
                    status = "Not Found",
                    message = "Organisation not found",
                    StatusCode = 404
                });

            }

            var userOrganisation = await _organisationService.GetUserOrganisationsAsync(userId);
            if(!userOrganisation.Any(o => o.OrgId == orgId))
            {
                return NotFound(new
                {
                    status = "Not Found",
                    message = "Organisation not found",
                    StatusCode = 404
                });
            }

            return Ok(new
            {
                status = "success",
                message = "Organisation retrieved successfully",
                data = new
                {
                    organisation.OrgId,
                    organisation.Name,
                    organisation.Description,
                }
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganisation([FromBody] OrganisationDto organisationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    status = "Bad Request",
                    message = "Client error",
                    statusCode = 400
                });
            }

            var userId = User.FindFirst("userId")?.Value;
            var organisation = await _organisationService.CreateOrganisationAsync(organisationDto.Name, organisationDto.Description, userId);

            return Created("", new
            {
                status = "success",
                message = "Organisation created successfully",
                data = new
                {
                    organisation.OrgId,
                    organisation.Name,
                    organisation.Description
                }
            });
        }

        [HttpPost("{orgId}/users")]
        public async Task<IActionResult> AddUserToOrganisation(string orgId, [FromBody] AddUserToOrganisationDto request)
        {
            var userExists = await _userOrganisationRepository.UserExistsInOrganisationAsync(request.UserId, orgId);
            if (userExists)
            {
                return BadRequest(new
                {
                    status = "Bad Request",
                    message = "User already assocaited to this organisation",
                    statusCode = 400
                });
            }

            await _organisationService.AddUserToOrganisationAsync(request.UserId, orgId);
            return Ok(new
            {
                status = "success",
                message = "User added to organisation successfully"
            });
        }
    }
}
