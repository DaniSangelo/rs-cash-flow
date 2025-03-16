using FluentAssertions;
using System.Net;

namespace WebApi.Test.Users;

public class DeleteUserTest : CashFlowClassFixture
{
    private const string RESOURCE = "api/User";
    private readonly string _token;

    public DeleteUserTest(CustomWebAppFactory webApp) : base(webApp)
    {
        _token = webApp.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var result = await SendDelete(RESOURCE, _token);
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
