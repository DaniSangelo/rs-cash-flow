using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Users;

public class ChangePasswordTest : CashFlowClassFixture
{
    private const string RESOURCE = "api/User/change-password";

    private readonly string _token;
    private readonly string _password;
    private readonly string _email;

    public ChangePasswordTest(CustomWebAppFactory webApp) : base(webApp)
    {
        _token = webApp.User_Team_Member.GetToken();
        _password = webApp.User_Team_Member.GetPassword();
        _email = webApp.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        string loginResource = "api/Login";
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;
        var response = await SendPut(RESOURCE, request, _token);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password,
        };
        response = await SendPost(loginResource, loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        loginRequest.Password = request.NewPassword;
        response = await SendPost(loginResource, loginRequest);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Password_Different_Current_Password(string culture)
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        var response = await SendPut(RESOURCE, request, token: _token, culture: culture);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errorMessage").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));
        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}
