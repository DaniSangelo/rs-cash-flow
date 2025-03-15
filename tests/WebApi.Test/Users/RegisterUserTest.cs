using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Users;

public class RegisterUserTest : CashFlowClassFixture
{
    private const string REGISTER_USER_RESOURCE = "api/User";
    public RegisterUserTest(CustomWebAppFactory webApp) : base(webApp){ }

    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var result = await SendPost(REGISTER_USER_RESOURCE, request);
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Error_Empty_Name()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        var result = await SendPost(REGISTER_USER_RESOURCE, request);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessage").EnumerateArray();
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(ResourceErrorMessages.NAME_EMPTY));
    }

    [Theory]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Empty_Name_With_Language(string cultureInfo)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;
        var result = await SendPost(resource: REGISTER_USER_RESOURCE, request: request, cultureInfo: cultureInfo);
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessage").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
