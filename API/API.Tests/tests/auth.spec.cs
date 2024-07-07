using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using API.Dto;
using API.Extensions;
using Newtonsoft.Json;
using Xunit;

namespace API.API.Tests.tests;

public class AuthTests : IClassFixture<TestFixture>
{
    private readonly HttpClient _client;

    public AuthTests(TestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task Register_Successful()
    {
        var registerDto = new RegisterDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Password = "P@ssw0rd",
            Phone = "1234567890"
        };

        var response = await _client.PostAsync("auth/register", new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json"));
        var responseBody = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<RegisterResponseDto>(responseBody);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("success", responseObject.Status);
        Assert.Equal("Registration successful", responseObject.Message);
        Assert.NotNull(responseObject.Data.AccessToken);
        Assert.Equal("John", responseObject.Data.User.FirstName);
    }

    [Fact]
    public async Task Should_Fail_If_Required_Fields_Are_Missing()
    {
        var registerDto = new RegisterDto
        {
            // Missing required fields intentionally
        };

        var response = await _client.PostAsync("auth/register", new StringContent(JsonConvert.SerializeObject(registerDto), Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var responseBody = await response.Content.ReadAsStringAsync();
        Assert.Contains("The FirstName field is required.", responseBody);
        Assert.Contains("The LastName field is required.", responseBody);
        Assert.Contains("The Email field is required.", responseBody);
        Assert.Contains("The Password field is required.", responseBody);
    }

    [Fact]
    public async Task Should_Fail_If_Duplicate_Email_Or_UserID()
    {
        var registerDto1 = new RegisterDto
        {
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            Password = "P@ssw0rd",
            Phone = "1234567890"
        };

        var registerDto2 = new RegisterDto
        {
            FirstName = "Alice",
            LastName = "Smith",
            Email = "jane.doe@example.com", // Same email as registerDto1
            Password = "P@ssw0rd",
            Phone = "0987654321"
        };

        var response1 = await _client.PostAsync("auth/register", new StringContent(JsonConvert.SerializeObject(registerDto1), Encoding.UTF8, "application/json"));
        var response2 = await _client.PostAsync("auth/register", new StringContent(JsonConvert.SerializeObject(registerDto2), Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Created, response1.StatusCode);
        Assert.Equal(HttpStatusCode.BadRequest, response2.StatusCode);
        var responseBody2 = await response2.Content.ReadAsStringAsync();
        Assert.Contains("Email already exists", responseBody2);
    }
}
