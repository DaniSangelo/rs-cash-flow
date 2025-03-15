using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace WebApi.Test.Users;

public class RegisterUserTest : IClassFixture<CustomWebAppFactory>
{
    private const string REGISTER_USER_RESOURCE = "api/User";
    private readonly HttpClient _httpClient;
    public RegisterUserTest(CustomWebAppFactory webApp)
    {
        _httpClient = webApp.CreateClient();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var result = await _httpClient.PostAsJsonAsync(REGISTER_USER_RESOURCE, request);
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
