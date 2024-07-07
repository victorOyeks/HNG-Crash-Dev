using API.Entities;
using API.Interfaces;
using API.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using Xunit;

namespace API.API.Tests.tests
{
    public class TokenServiceTest
    {
        private readonly ITokenService _tokenService;
        private readonly Mock<IConfiguration> _configurationMock;

        public TokenServiceTest()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.SetupGet(x => x["JWT:SigningKey"]).Returns("thisisjsutarandomtestsecretkeyifjsutarandomtestsecretkeyifjsutarandomtestsecre");
            _configurationMock.SetupGet(x => x["JWT:Issuer"]).Returns("http://localhost:5246");
            _configurationMock.SetupGet(x => x["JWT:Audience"]).Returns("http://localhost:5246");

            _tokenService = new TokenService(_configurationMock.Object);
        }

        [Fact]
        public void GenerateToken_ShouldContainCorrectUserDetails()
        {
            // Arrange
            var user = new AppUser
            {
                UserId = "434229ae-738f-4482-bb65-42dfa100115e",
                Email = "example@email.com",
                FirstName = "James",
                LastName = "Bond"
            };

            var token = _tokenService.CreateToken(user);
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token) as JwtSecurityToken;

            Assert.NotNull(jwtToken);
            Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.Email && c.Value == user.Email);
            Assert.Contains(jwtToken.Claims, c => c.Type == "userId" && c.Value == user.UserId);
            Assert.Contains(jwtToken.Claims, c => c.Type == JwtRegisteredClaimNames.GivenName && c.Value == user.FirstName);

            // Check token expiration
            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);
            Assert.NotNull(expClaim);

            var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value)).UtcDateTime;
            var expectedExpiration = DateTime.UtcNow.AddDays(1);

            // Assert expiration time is approximately as expected (within a reasonable tolerance)
            Assert.True((expectedExpiration - exp).TotalMinutes < 1, "Token expiration time is not as expected.");
        }
    }
}
