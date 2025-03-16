using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Users;

public class UpdateUserTest : CashFlowClassFixture
{
    private const string RESOURCE = "api/User";
    private readonly string _token;

    public UpdateUserTest(CustomWebAppFactory webApp) : base(webApp)
    {
        _token = webApp.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var response = await SendPut(RESOURCE, request, token: _token);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Empty_Name(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;
        var response = await SendPut(RESOURCE, request, token: _token, culture: culture);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errorMessage").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));
        errors.Should().HaveCount(1).And.Contain(c => c.GetString()!.Equals(expectedMessage));
    }
}
