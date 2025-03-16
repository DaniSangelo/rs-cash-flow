using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace WebApi.Test.Users;

public class GetUserProfileTest : CashFlowClassFixture
{
    private const string RESOURCE = "api/User";

    private readonly string _token;
    private readonly string _userName;
    private readonly string _userEmail;

    public GetUserProfileTest(CustomWebAppFactory webApp) : base(webApp)
    {
        _token = webApp.User_Team_Member.GetToken();
        _userName = webApp.User_Team_Member.GetName();
        _userEmail = webApp.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var result = await SendGet(RESOURCE, _token);
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        response.RootElement.GetProperty("name").GetString().Should().Be(_userName);
        response.RootElement.GetProperty("email").GetString().Should().Be(_userEmail);
    }
}
