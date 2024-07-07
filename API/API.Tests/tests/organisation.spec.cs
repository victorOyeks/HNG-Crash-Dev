using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dto;
using API.Entities;
using API.Repositories;
using API.Services;
using Moq;
using Xunit;

namespace API.API.Tests.tests
{
    public class OrganisationServiceTest
    {
        [Fact]
        public async Task GetOrganisationByIdAsync_UserCannotAccess_OrganisationNotFound()
        {
            var userId = "abaac316-737d-4a81-ae14-99de4d80c692";
            var organisationId = "efc55c59-6679-4c76-bfdb-d7f8dff8c8e3";
            var mockUserRepository = new Mock<IUserRepository>();
            var mockOrganisationRepository = new Mock<IOrganisationRepository>();
            var mockUserOrganisationRepository = new Mock<IUserOrganisationRepository>();

            mockUserRepository.Setup(repo => repo.GetUserByIdAsync(userId))
                .ReturnsAsync(new AppUser { UserId = userId });

            mockOrganisationRepository.Setup(repo => repo.GetOrganisationByIdAsync(organisationId))
                .ReturnsAsync(new Organisation { OrgId = organisationId, Name = "Test Org", Description = "Test Description" });

            mockUserOrganisationRepository.Setup(repo => repo.UserExistsInOrganisationAsync(userId, organisationId))
                .ReturnsAsync(false);

            var organisationService = new OrganisationService(
                mockOrganisationRepository.Object,
                mockUserRepository.Object,
                mockUserOrganisationRepository.Object
            );

            var result = await organisationService.GetUserOrganisationAsync(userId, organisationId);

            Assert.Null(result);
        }
    }
}
